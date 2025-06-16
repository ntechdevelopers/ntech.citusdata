public async Task<IQueryable<ProjectSuggestionDto>> GetProjectSuggestionsAsync(GetProjectSuggestionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetProjectSuggestionsAsync - request={JsonHelper.PrettySerialize(request)}");

            var userId = _securityContext.UserGuidId;
            var followedProjectIds = await GetFollowedProjectIdsByUserId(userId);

            var currentDate = DateTime.UtcNow.ToUnixTime();
            var projectSuggestionsQuery = _context.ProjectSuggestions
                .AsNoTracking()
                .Where(x => !x.IsDeleted
                    && x.UserId == userId
                    && x.EnableAfterDatetime < currentDate
                    && !followedProjectIds.Contains(x.ProjectId))
                .Select(x => new
                {
                    ProjectId = x.ProjectId,
                    ProjectSuggestionCreatedDatetime = x.CreatedDatetime
                });

            if (projectSuggestionsQuery.FirstOrDefault() == null)
            {
                _logger.LogWarning($"GetProjectSuggestionsAsync - Cannot get project suggession by user id.");
                return new List<ProjectSuggestionDto>().AsQueryable();
            }

            return _context
                    .Projects
                    .Include(x => x.ProjectAddress)
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted
                        && x.ProjectPostStatusId == ProjectPostStatusKey.Public)
                    .Join(projectSuggestionsQuery,
                        project => project.Id,
                        projectSuggestion => projectSuggestion.ProjectId,
                        (project, projectSuggestion) => new ProjectSuggestionDto
                        {
                            ProjectId = project.Id.ToString(),
                            ProjectName = project.ProjectName,
                            ProjectDescription = project.ProjectDescription,
                            ProjectStatusId = project.ProjectStatusId.HasValue ? project.ProjectStatusId.Value.ToString() : null,
                            ProjectStatusName = _projectStatusFunc(project.ProjectStatusId).ProjectStatusName,
                            ProjectStatusDescription = _projectStatusFunc(project.ProjectStatusId).ProjectStatusDescription,
                            PropertyTypeId = project.PropertyTypeId.HasValue ? project.PropertyTypeId.Value.ToString() : null,
                            PropertyTypeName = _propertyTypeFunc(project.PropertyTypeId).PropertyTypeName,
                            PropertyTypeDescription = _propertyTypeFunc(project.PropertyTypeId).PropertyTypeDescription,
                            InvestorOwnerName = project.InvestorOwnerName,
                            ProjectProgress = project.ProjectProgress,
                            MinPrice = project.MinPrice.HasValue ? (double)project.MinPrice.Value : (double?)null,
                            FeaturePhotos = project.FeaturePhotos,
                            CommissionRates = project.CommissionRates,
                            ProjectCode = project.ProjectCode,
                            IsFollowed = false,
                            ProjectSuggestionCreatedDatetime = projectSuggestion.ProjectSuggestionCreatedDatetime,

                            ProjectAddressCityId = project.ProjectAddress != null ? project.ProjectAddress.CityId : (int?)null,
                            ProjectAddressDistrictId = project.ProjectAddress != null ? project.ProjectAddress.DistrictId : (int?)null,
                            ProjectAddress = project.ProjectAddress == null ? new ProjectAddressInfoDto() : new ProjectAddressInfoDto
                            {
                                CountryId = project.ProjectAddress.CountryId,
                                CityId = project.ProjectAddress.CityId,
                                DistrictId = project.ProjectAddress.DistrictId,
                                WardId = project.ProjectAddress.WardId,

                                CountryName = project.ProjectAddress.CountryId != null ? _countryFunc(project.ProjectAddress.CountryId ?? 0).CountryName : null,
                                CityName = project.ProjectAddress.CityId != null ? _cityFunc(project.ProjectAddress.CityId ?? 0).CityName : null,
                                DistrictName = project.ProjectAddress.DistrictId != null ? _districtFunc(project.ProjectAddress.DistrictId ?? 0).DistrictName : null,
                                WardName = project.ProjectAddress.WardId != null ? _wardFunc(project.ProjectAddress.WardId ?? 0).WardName : null,

                                StreetName = project.ProjectAddress.StreetName,
                                HomeAddress = project.ProjectAddress.HomeAddress,
                                Building = project.ProjectAddress.Building,
                                Floor = project.ProjectAddress.Floor,
                                Block = project.ProjectAddress.Block,
                                Room = project.ProjectAddress.Room,
                                AdditionalLocationInfo = project.ProjectAddress.AdditionalLocationInfo,
                                Landmark = project.ProjectAddress.Landmark,
                                Longitude = project.ProjectAddress.Longitude,
                                Latitude = project.ProjectAddress.Latitude,

                                GoogleEmbedCode = project.ProjectAddress.GoogleEmbedCode,
                                GoogleShareLink = project.ProjectAddress.GoogleShareLink,
                                ImageMapUrl = project.ProjectAddress.ImageMapURL,
                            }
                        });
        }