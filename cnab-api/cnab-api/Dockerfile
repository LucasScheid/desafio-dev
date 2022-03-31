#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["cnab-api/cnab-api.csproj", "cnab-api/"]
COPY ["cnab-services/cnab-services.csproj", "cnab-services/"]
COPY ["cnab-entities/cnab-entities.csproj", "cnab-entities/"]
COPY ["cnab-infra/cnab-infra.csproj", "cnab-infra/"]
COPY ["cnab-contracts/cnab-contracts.csproj", "cnab-contracts/"]
COPY ["cnab-helpers/cnab-helpers.csproj", "cnab-helpers/"]
RUN dotnet restore "cnab-api/cnab-api.csproj"
COPY . .
WORKDIR "/src/cnab-api"
RUN dotnet build "cnab-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cnab-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cnab-api.dll"]