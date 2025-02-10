FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7005
EXPOSE 8005

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SearchService/SearchService.csproj", "SearchService/"]
COPY ["Marketplace.Shared/Marketplace.Shared.csproj", "Marketplace.Shared/"]
RUN dotnet restore "./SearchService/SearchService.csproj"
COPY . .
WORKDIR "/src/SearchService"
RUN dotnet build "./SearchService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SearchService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchService.dll"]