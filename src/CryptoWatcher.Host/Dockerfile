FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CryptoWatcher.Host/CryptoWatcher.Host.csproj", "src/CryptoWatcher.Host/"]
COPY ["src/modules/uniswap/CryptoWatcher.Modules.Uniswap.Infrastructure/CryptoWatcher.Modules.Uniswap.Infrastructure.csproj", "src/modules/uniswap/CryptoWatcher.Modules.Uniswap.Infrastructure/"]
COPY ["src/modules/uniswap/CryptoWatcher.Modules.Uniswap.Application/CryptoWatcher.Modules.Uniswap.Application.csproj", "src/modules/uniswap/CryptoWatcher.Modules.Uniswap.Application/"]
COPY ["src/modules/uniswap/CryptoWatcher.Modules.Uniswap/CryptoWatcher.Modules.Uniswap.csproj", "src/modules/uniswap/CryptoWatcher.Modules.Uniswap/"]
COPY ["src/CryptoWatcher/CryptoWatcher.csproj", "src/CryptoWatcher/"]
COPY ["clients/AaveClient/AaveClient.csproj", "clients/AaveClient/"]
COPY ["clients/CoinGeckoClient/CoinGeckoClient.csproj", "clients/CoinGeckoClient/"]
COPY ["src/CryptoWatcher.Application/CryptoWatcher.Application.csproj", "src/CryptoWatcher.Application/"]
COPY ["src/modules/aave/CryptoWatcher.AaveModule/CryptoWatcher.AaveModule.csproj", "src/modules/aave/CryptoWatcher.AaveModule/"]
COPY ["src/modules/hyperliquid/CryptoWatcher.HyperliquidModule/CryptoWatcher.HyperliquidModule.csproj", "src/modules/hyperliquid/CryptoWatcher.HyperliquidModule/"]
COPY ["clients/HyperliquidClient/HyperliquidClient.csproj", "clients/HyperliquidClient/"]
COPY ["src/CryptoWatcher.Infrastructure/CryptoWatcher.Infrastructure.csproj", "src/CryptoWatcher.Infrastructure/"]
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
