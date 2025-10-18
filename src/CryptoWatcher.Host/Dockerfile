FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CryptoWatcher.Host/CryptoWatcher.Host.csproj", "src/CryptoWatcher.Host/"]
COPY ["src/modules/hyperliquid/CryptoWatcher.Modules.Hyperliquid.Infrastructure/CryptoWatcher.Modules.Hyperliquid.Infrastructure.csproj", "src/modules/hyperliquid/CryptoWatcher.Modules.Hyperliquid.Infrastructure/"]
COPY ["src/modules/hyperliquid/CryptoWatcher.Modules.Hyperliquid.Application/CryptoWatcher.Modules.Hyperliquid.Application.csproj", "src/modules/hyperliquid/CryptoWatcher.Modules.Hyperliquid.Application/"]
COPY ["src/modules/hyperliquid/CryptoWatcher.Modules.Hyperliquid/CryptoWatcher.Modules.Hyperliquid.csproj", "src/modules/hyperliquid/CryptoWatcher.Modules.Hyperliquid/"]
COPY ["src/CryptoWatcher/CryptoWatcher.csproj", "src/CryptoWatcher/"]
COPY ["src/modules/uniswap/CryptoWatcher.Modules.Uniswap.Infrastructure/CryptoWatcher.Modules.Uniswap.Infrastructure.csproj", "src/modules/uniswap/CryptoWatcher.Modules.Uniswap.Infrastructure/"]
COPY ["src/modules/uniswap/CryptoWatcher.Modules.Uniswap.Application/CryptoWatcher.Modules.Uniswap.Application.csproj", "src/modules/uniswap/CryptoWatcher.Modules.Uniswap.Application/"]
COPY ["src/modules/uniswap/CryptoWatcher.Modules.Uniswap/CryptoWatcher.Modules.Uniswap.csproj", "src/modules/uniswap/CryptoWatcher.Modules.Uniswap/"]
COPY ["clients/CoinGeckoClient/CoinGeckoClient.csproj", "clients/CoinGeckoClient/"]
COPY ["src/CryptoWatcher.Application/CryptoWatcher.Application.csproj", "src/CryptoWatcher.Application/"]
COPY ["src/CryptoWatcher.Infrastructure/CryptoWatcher.Infrastructure.csproj", "src/CryptoWatcher.Infrastructure/"]
COPY ["src/modules/aave/CryptoWatcher.Modules.Aave/CryptoWatcher.Modules.Aave.csproj", "src/modules/aave/CryptoWatcher.Modules.Aave/"]
COPY ["src/modules/aave/CryptoWatcher.Modules.Aave.Infrastructure/CryptoWatcher.Modules.Aave.Infrastructure.csproj", "src/modules/aave/CryptoWatcher.Modules.Aave.Infrastructure/"]
COPY ["src/modules/aave/CryptoWatcher.Modules.Aave.Application/CryptoWatcher.Modules.Aave.Application.csproj", "src/modules/aave/CryptoWatcher.Modules.Aave.Application/"]
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
