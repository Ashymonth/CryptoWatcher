using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChainIdAndStateViewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmartContractAddresses_NftManager",
                table: "UniswapChainConfigurations");
            
            migrationBuilder.AddColumn<int>(
                name: "ChainId",
                table: "UniswapChainConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmartContractAddresses_StateView",
                table: "UniswapChainConfigurations",
                type: "character(42)",
                fixedLength: true,
                maxLength: 42,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChainId",
                table: "UniswapChainConfigurations");

            migrationBuilder.DropColumn(
                name: "SmartContractAddresses_StateView",
                table: "UniswapChainConfigurations");
        }
    }
}
