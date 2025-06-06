FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CryptoWatcher.Host/CryptoWatcher.Host.csproj", "src/CryptoWatcher.Host/"]
COPY ["clients/UniswapClient/UniswapClient.csproj", "clients/UniswapClient/"]
COPY ["src/CryptoWatcher.Application/CryptoWatcher.Application.csproj", "src/CryptoWatcher.Application/"]
COPY ["src/CryptoWatcher/CryptoWatcher.csproj", "src/CryptoWatcher/"]
COPY ["src/CryptoWatcher.Data/CryptoWatcher.Data.csproj", "src/CryptoWatcher.Data/"]
RUN dotnet restore "src/CryptoWatcher.Host/CryptoWatcher.Host.csproj"
COPY . .
WORKDIR "/src/src/CryptoWatcher.Host"
RUN dotnet build "./CryptoWatcher.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CryptoWatcher.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoWatcher.Host.dll"]
