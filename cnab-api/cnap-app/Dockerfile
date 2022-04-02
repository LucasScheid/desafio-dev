#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["cnap-app/cnap-app.csproj", "cnap-app/"]
COPY ["cnab-helpers/cnab-helpers.csproj", "cnab-helpers/"]
COPY ["cnab-entities/cnab-entities.csproj", "cnab-entities/"]
COPY ["cnab-contracts/cnab-contracts.csproj", "cnab-contracts/"]
RUN dotnet restore "cnap-app/cnap-app.csproj"
COPY . .
WORKDIR "/src/cnap-app"
RUN dotnet build "cnap-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cnap-app.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cnap-app.dll"]