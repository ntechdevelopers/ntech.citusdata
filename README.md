Citus Data Setup

Bài viết
http://blog.ntechdevelopers.com/xay-dung-web-app-multi-tenant-don-gian-voi-citus-data-va-aspnet-core/

- psql connection-string
- docker exec -it citus_master psql -U postgres

- cd E:\LocalGit\NtechAzure\ntech.citusdata
- docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build
- docker ps
- docker exec -it postgresql bash

- su postgres
- psql
- psql -f .\init.sql
- psql -f .\sharding.sql
- psql -f .\seeding.sql


- dotnet new mvc -o Ntech.CitusData
- cd Ntech.CitusData
- dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
- dotnet new sln -o Ntech.CitusData
- dotnet sln Ntech.CitusData add .\Ntech.CitusData\Ntech.CitusData.csproj
- cd Ntech.CitusData

- dotnet add package Microsoft.AspNetCore.App
- dotnet add package SaasKit.Multitenancy

# Update host for domain
- 127.0.0.1	ntechdevelopers.local
- 127.0.0.1	api.ntechdevelopers.local

# blog.ntechdevelopers.com

http://blog.ntechdevelopers.com/citus-data-tuong-de-ma-kho-khong-tuong/
