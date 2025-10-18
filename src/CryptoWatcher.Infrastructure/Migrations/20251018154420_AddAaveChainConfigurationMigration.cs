using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAaveChainConfigurationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HyperliquidVaultPositionSnapshots_Wallets_WalletAddress",
                table: "HyperliquidVaultPositionSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_HyperliquidVaultPositionSnapshots_WalletAddress",
                table: "HyperliquidVaultPositionSnapshots");

            migrationBuilder.CreateTable(
                name: "AaveChainConfigurations",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RpcUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RpcAuthToken = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    SmartContractAddresses_PoolAddressesProviderAddress = table.Column<string>(type: "character(42)", fixedLength: true, maxLength: 42, nullable: false),
                    SmartContractAddresses_UiPoolDataProviderAddress = table.Column<string>(type: "character(42)", fixedLength: true, maxLength: 42, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AaveChainConfigurations", x => x.Name);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AaveChainConfigurations");

            migrationBuilder.CreateIndex(
                name: "IX_HyperliquidVaultPositionSnapshots_WalletAddress",
                table: "HyperliquidVaultPositionSnapshots",
                column: "WalletAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_HyperliquidVaultPositionSnapshots_Wallets_WalletAddress",
                table: "HyperliquidVaultPositionSnapshots",
                column: "WalletAddress",
                principalTable: "Wallets",
                principalColumn: "Address",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
