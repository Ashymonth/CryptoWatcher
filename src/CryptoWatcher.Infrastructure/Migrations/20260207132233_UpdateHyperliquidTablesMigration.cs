using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHyperliquidTablesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HyperliquidPositionCashFlows_Wallets_WalletAddress",
                table: "HyperliquidPositionCashFlows");

            migrationBuilder.DropIndex(
                name: "IX_HyperliquidPositionCashFlows_WalletAddress",
                table: "HyperliquidPositionCashFlows");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "HyperliquidVaultPositions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HyperliquidVaultPositions");

            migrationBuilder.DropColumn(
                name: "InitialBalance",
                table: "HyperliquidVaultPositions");

            migrationBuilder.AddColumn<string>(
                name: "TransactionHash",
                table: "HyperliquidPositionCashFlows",
                type: "character(66)",
                unicode: false,
                fixedLength: true,
                maxLength: 66,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HyperliquidSynchronizationStates",
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
                name: "HyperliquidVaultPeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WalletAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false),
                    VaultAddress = table.Column<string>(type: "character(42)", unicode: false, fixedLength: true, maxLength: 42, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidVaultPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HyperliquidVaultPeriod_HyperliquidVaultPositions_VaultAddre~",
                        columns: x => new { x.VaultAddress, x.WalletAddress },
                        principalTable: "HyperliquidVaultPositions",
                        principalColumns: new[] { "VaultAddress", "WalletAddress" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HyperliquidVaultPeriod_VaultAddress_WalletAddress",
                table: "HyperliquidVaultPeriod",
                columns: new[] { "VaultAddress", "WalletAddress" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HyperliquidSynchronizationStates");

            migrationBuilder.DropTable(
                name: "HyperliquidVaultPeriod");

            migrationBuilder.DropColumn(
                name: "TransactionHash",
                table: "HyperliquidPositionCashFlows");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ClosedAt",
                table: "HyperliquidVaultPositions",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "HyperliquidVaultPositions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<decimal>(
                name: "InitialBalance",
                table: "HyperliquidVaultPositions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_HyperliquidPositionCashFlows_WalletAddress",
                table: "HyperliquidPositionCashFlows",
                column: "WalletAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_HyperliquidPositionCashFlows_Wallets_WalletAddress",
                table: "HyperliquidPositionCashFlows",
                column: "WalletAddress",
                principalTable: "Wallets",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
