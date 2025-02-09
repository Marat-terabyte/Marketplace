FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7003


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotifictationService/NotifictationService.csproj", "NotifictationService/"]
COPY ["Marketplace.Shared/Marketplace.Shared.csproj", "Marketplace.Shared/"]
RUN dotnet restore "./NotifictationService/NotifictationService.csproj"
COPY . .
WORKDIR "/src/NotifictationService"
RUN dotnet build "./NotifictationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NotifictationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotifictationService.dll"]