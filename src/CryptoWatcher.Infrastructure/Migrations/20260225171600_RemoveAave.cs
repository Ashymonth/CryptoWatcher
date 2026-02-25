using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AaveAccountSnapshots");

            migrationBuilder.DropTable(
                name: "AaveChainConfigurations");

            migrationBuilder.DropTable(
                name: "AavePositionCashFlows");

            migrationBuilder.DropTable(
                name: "AavePositionDailyPerformances");

            migrationBuilder.DropTable(
                name: "AavePositionPeriod");

            migrationBuilder.DropTable(
                name: "AavePositionSnapshots");

            migrationBuilder.DropTable(
                name: "AavePositions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AaveAccountSnapshots",
                columns: table => new
                {
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    NetworkName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    HealthFactor = table.Column<double>(type: "double precision", nullable: false),
                    TotalCollateralInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalDebtInUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AaveAccountSnapshots", x => new { x.WalletAddress, x.NetworkName, x.Day });
                });

            migrationBuilder.CreateTable(
                name: "AaveChainConfigurations",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RpcAuthToken = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    RpcUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SmartContractAddresses_PoolAddressesProviderAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    SmartContractAddresses_UiPoolDataProviderAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AaveChainConfigurations", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "AavePositionDailyPerformances",
                columns: table => new
                {
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    PositionType = table.Column<int>(type: "integer", nullable: false),
                    SnapshotPositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    NetworkName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ProfitInToken = table.Column<decimal>(type: "numeric", nullable: false),
                    ProfitInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    Token0_Address = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_Symbol = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AavePositionDailyPerformances", x => new { x.Day, x.PositionType, x.SnapshotPositionId });
                });

            migrationBuilder.CreateTable(
                name: "AavePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    Network = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PositionType = table.Column<int>(type: "integer", nullable: false),
                    PreviousScaledAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    Token0_Address = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_Symbol = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AavePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AavePositions_Wallets_WalletAddress",
                        column: x => x.WalletAddress,
                        principalTable: "Wallets",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AavePositionCashFlows",
                columns: table => new
                {
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Event = table.Column<int>(type: "integer", nullable: false),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AavePositionCashFlows", x => new { x.PositionId, x.Date, x.Event });
                    table.ForeignKey(
                        name: "FK_AavePositionCashFlows_AavePositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "AavePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AavePositionPeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClosedAtDay = table.Column<DateOnly>(type: "date", nullable: true),
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAtDay = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AavePositionPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AavePositionPeriod_AavePositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "AavePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AavePositionSnapshots",
                columns: table => new
                {
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    LiquidationLtv = table.Column<double>(type: "double precision", nullable: true),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AavePositionSnapshots", x => new { x.PositionId, x.Day });
                    table.ForeignKey(
                        name: "FK_AavePositionSnapshots_AavePositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "AavePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AavePositionPeriod_PositionId_ClosedAtDay",
                table: "AavePositionPeriod",
                columns: new[] { "PositionId", "ClosedAtDay" },
                unique: true,
                filter: " \"ClosedAtDay\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AavePositions_WalletAddress",
                table: "AavePositions",
                column: "WalletAddress");
        }
    }
}
