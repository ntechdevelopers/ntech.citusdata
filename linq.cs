public IQueryable<ProjectDto> GetProjectsForPublicAsync(CancellationToken cancellationToken)
        {
            return _context.Projects
                .Include(x => x.ProjectAddress)
                .Include(x => x.SaleSeasons)
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.ProjectPostStatusId.HasValue && x.ProjectPostStatusId.Value == ProjectPostStatusKey.Public)
                .Distinct()
                .Select(project => new ProjectDto
                {
                    ProjectId = project.Id.ToString(),
                    ProjectCode = project.ProjectCode,
                    ProjectName = project.ProjectName,
                    PropertyTypeId = project.PropertyTypeId.HasValue ? project.PropertyTypeId.ToString() : string.Empty,
                    ProjectAddressId = project.ProjectAddressId.HasValue ? project.ProjectAddressId.ToString() : string.Empty,
                    ProjectStatusId = project.ProjectStatusId.HasValue ? project.ProjectStatusId.ToString() : string.Empty,
                    ProjectPostStatusId = project.ProjectPostStatusId.HasValue ? project.ProjectPostStatusId.ToString() : string.Empty,
                    ProjectProgress = project.ProjectProgress,
                    ProjectDescription = project.ProjectDescription,
                    FeaturePhotos = project.FeaturePhotos,
                    IsFeaturesProject = project.IsFeaturesProject,
                    InvestorOwnerName = project.InvestorOwnerName,
                    InvestorOwnerLogo = project.InvestorOwnerLogo,
                    InvestorOwnerInfo = project.InvestorOwnerInfo,
                    CommissionRates = project.CommissionRates,
                    PartnersInfo = project.PartnersInfo,
                    BankInfo = project.BankInfo,
                    OverviewDescription = project.OverviewDescription,
                    OverviewMediaInfo = project.OverviewMediaInfo,
                    SizingDescription = project.SizingDescription,
                    SizingMediaInfo = project.SizingMediaInfo,
                    LocationDescription = project.LocationDescription,
                    LocationMediaInfo = project.LocationMediaInfo,
                    FacilitiesDescription = project.FacilitiesDescription,
                    FacilitiesMediaInfo = project.FacilitiesMediaInfo,
                    GroundPlanDescription = project.GroundPlanDescription,
                    GroundPlanMediaInfo = project.GroundPlanMediaInfo,
                    SaleProgramDescription = project.SaleProgramDescription,
                    SaleProgramMediaInfo = project.SaleProgramMediaInfo,
                    CreatedDatetime = project.CreatedDatetime,
                    CreatedBy = project.CreatedByUserId.ToString(),
                    LastModified = project.UpdatedDatetime,
                    ModifiedBy = project.UpdatedByUserId.HasValue ? project.UpdatedByUserId.Value.ToString() : null,
                    DeletedDatetime = project.DeletedDatetime,
                    DeletedBy = project.DeletedByUserId.HasValue ? project.DeletedByUserId.Value.ToString() : null,
                    IsDeleted = project.IsDeleted,
                    TotalOfSaleSeasons = project.SaleSeasons.Count(s => !s.IsDeleted),
                    SortOrder = project.SortOrder,
                    MinPrice = project.MinPrice.ToDouble(),

                    ProjectAddress = Helpers.ProjectAddressHelper.GetAddressInfoDto(project.ProjectAddress,
                        _localDataService.Countries, _localDataService.Cities, _localDataService.Districts, _localDataService.Wards),

                    ProjectStatusName = _projectStatusFunc(project.ProjectStatusId).ProjectStatusName,
                    ProjectStatusDescription = _projectStatusFunc(project.ProjectStatusId).ProjectStatusDescription,

                    ProjectPostStatusName = _projectPostStatusFunc(project.ProjectPostStatusId).ProjectPostStatusName,
                    ProjectPostStatusDescription = _projectPostStatusFunc(project.ProjectPostStatusId).ProjectPostStatusDescription,

                    ProjectTypeName = _propertyTypeFunc(project.PropertyTypeId).PropertyTypeName,
                    ProjectTypeDescription = _propertyTypeFunc(project.PropertyTypeId).PropertyTypeDescription,

                    TotalFollower = SaleContext.CountTotalFollowerByProjectId(project.Id, _context),
                    TotalShare = SaleContext.CountTotalShareByProjectId(project.Id, _context),
                });
        }