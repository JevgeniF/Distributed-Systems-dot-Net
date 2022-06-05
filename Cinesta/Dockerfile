FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /app
EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

#App
COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.DTO/*.csproj ./App.BLL.DTO/
COPY App.Contracts.BLL/*.csproj ./App.Contracts.BLL/
COPY App.Contracts.DAL/*.csproj ./App.Contracts.DAL/
COPY App.Contracts.Public/*.csproj ./App.Contracts.Public/
COPY App.DAL.DTO/*.csproj ./App.DAL.DTO/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY App.Public/*.csproj ./App.Public/
COPY App.Public.DTO.v1/*.csproj ./App.Public.DTO.v1/
COPY App.Resources/*.csproj ./App.Resources/
COPY WebApp/*.csproj ./WebApp/

#Base
COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Extensions/*.csproj ./Base.Extensions/
COPY Base.Public/*.csproj ./Base.Public/
COPY Base.Resources/*.csproj ./Base.Resources/
COPY Helpers.WebApp/*.csproj ./Helpers.WebApp/

#Base.Contracts
COPY Base.Contracts.BLL/*.csproj ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/*.csproj ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/*.csproj ./Base.Contracts.Domain/
COPY Base.Contracts.Mapper/*.csproj ./Base.Contracts.Mapper/
COPY Base.Contracts.Public/*.csproj ./Base.Contracts.Public/

COPY IntTests/*.csproj ./IntTests/
COPY UnitTests/*.csproj ./UnitTests/

RUN dotnet restore

# copy everything else and build app

#App
COPY App.BLL/. ./App.BLL/
COPY App.BLL.DTO/. ./App.BLL.DTO/
COPY App.Contracts.BLL/. ./App.Contracts.BLL/
COPY App.Contracts.DAL/. ./App.Contracts.DAL/
COPY App.Contracts.Public/. ./App.Contracts.Public/
COPY App.DAL.DTO/. ./App.DAL.DTO/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY App.Public/. ./App.Public/
COPY App.Public.DTO.v1/. ./App.Public.DTO.v1/
COPY App.Resources/. ./App.Resources/
COPY WebApp/. ./WebApp/

#Base
COPY Base.BLL/. ./Base.BLL/
COPY Base.DAL/. ./Base.DAL/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.Extensions/. ./Base.Extensions/
COPY Base.Public/. ./Base.Public/
COPY Base.Resources/. ./Base.Resources/
COPY Helpers.WebApp/. ./Helpers.WebApp/

#Base.Contracts
COPY Base.Contracts.BLL/. ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/. ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/. ./Base.Contracts.Domain/
COPY Base.Contracts.Mapper/. ./Base.Contracts.Mapper/
COPY Base.Contracts.Public/. ./Base.Contracts.Public/

COPY IntTests/. ./IntTests/
COPY UnitTests/. ./UnitTests

WORKDIR /app/WebApp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:latest AS runtime
WORKDIR /app
EXPOSE 80
ENV ConnectionStrings:SqlServerConnection="Host=cinestadb.postgres.database.azure.com;Port=5432;Database=cinesta;Username=postgres@cinestadb;Password=DataBase321"
COPY --from=build /app/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll"]