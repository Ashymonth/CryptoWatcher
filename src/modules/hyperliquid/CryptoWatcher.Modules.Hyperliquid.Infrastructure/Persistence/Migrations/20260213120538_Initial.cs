using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "hyperliquid");

            migrationBuilder.CreateTable(
                name: "HyperliquidPositionDailyPerformances",
                schema: "hyperliquid",
                columns: table => new
                {
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    VaultAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    BalanceInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    ProfitInUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidPositionDailyPerformances", x => new { x.VaultAddress, x.WalletAddress, x.Day });
                });

            migrationBuilder.CreateTable(
                name: "HyperliquidSynchronizationStates",
                schema: "hyperliquid",
                columns: table => new
                {
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    LastProcessedEventTimestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastTransactionHash = table.Column<string>(type: "character(66)", unicode: false, fixedLength: true, maxLength: 66, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidSynchronizationStates", x => x.WalletAddress);
                });

            migrationBuilder.CreateTable(
                name: "HyperliquidVaultPositions",
                schema: "hyperliquid",
                columns: table => new
                {
                    VaultAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    Token0_Address = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidVaultPositions", x => new { x.VaultAddress, x.WalletAddress });
                });

            migrationBuilder.CreateTable(
                name: "HyperliquidPositionCashFlows",
                schema: "hyperliquid",
                columns: table => new
                {
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    VaultAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    Event = table.Column<int>(type: "integer", nullable: false),
                    TransactionHash = table.Column<string>(type: "character(66)", unicode: false, fixedLength: true, maxLength: 66, nullable: false),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidPositionCashFlows", x => new { x.VaultAddress, x.WalletAddress, x.Date });
                    table.ForeignKey(
                        name: "FK_HyperliquidPositionCashFlows_HyperliquidVaultPositions_Vaul~",
                        columns: x => new { x.VaultAddress, x.WalletAddress },
                        principalSchema: "hyperliquid",
                        principalTable: "HyperliquidVaultPositions",
                        principalColumns: new[] { "VaultAddress", "WalletAddress" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HyperliquidVaultPeriod",
                schema: "hyperliquid",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ClosedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    VaultAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidVaultPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HyperliquidVaultPeriod_HyperliquidVaultPositions_VaultAddre~",
                        columns: x => new { x.VaultAddress, x.WalletAddress },
                        principalSchema: "hyperliquid",
                        principalTable: "HyperliquidVaultPositions",
                        principalColumns: new[] { "VaultAddress", "WalletAddress" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HyperliquidVaultPositionSnapshots",
                schema: "hyperliquid",
                columns: table => new
                {
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    VaultAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidVaultPositionSnapshots", x => new { x.VaultAddress, x.WalletAddress, x.Day });
                    table.ForeignKey(
                        name: "FK_HyperliquidVaultPositionSnapshots_HyperliquidVaultPositions~",
                        columns: x => new { x.VaultAddress, x.WalletAddress },
                        principalSchema: "hyperliquid",
                        principalTable: "HyperliquidVaultPositions",
                        principalColumns: new[] { "VaultAddress", "WalletAddress" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HyperliquidVaultPeriod_VaultAddress_WalletAddress",
                schema: "hyperliquid",
                table: "HyperliquidVaultPeriod",
                columns: new[] { "VaultAddress", "WalletAddress" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HyperliquidPositionCashFlows",
                schema: "hyperliquid");

            migrationBuilder.DropTable(
                name: "HyperliquidPositionDailyPerformances",
                schema: "hyperliquid");

            migrationBuilder.DropTable(
                name: "HyperliquidSynchronizationStates",
                schema: "hyperliquid");

            migrationBuilder.DropTable(
                name: "HyperliquidVaultPeriod",
                schema: "hyperliquid");

            migrationBuilder.DropTable(
                name: "HyperliquidVaultPositionSnapshots",
                schema: "hyperliquid");

            migrationBuilder.DropTable(
                name: "HyperliquidVaultPositions",
                schema: "hyperliquid");
        }
    }
}
