FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7006
EXPOSE 8006


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CartService/CartService.csproj", "CartService/"]
COPY ["Marketplace.Shared/Marketplace.Shared.csproj", "Marketplace.Shared/"]
RUN dotnet restore "./CartService/CartService.csproj"
COPY . .
WORKDIR "/src/CartService"
RUN dotnet build "./CartService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CartService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CartService.dll"]