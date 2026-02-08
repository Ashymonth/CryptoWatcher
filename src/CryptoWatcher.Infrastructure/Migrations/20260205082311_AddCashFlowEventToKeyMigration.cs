using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCashFlowEventToKeyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UniswapLiquidityPositionCashFlows",
                table: "UniswapLiquidityPositionCashFlows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniswapLiquidityPositionCashFlows",
                table: "UniswapLiquidityPositionCashFlows",
                columns: new[] { "PositionId", "NetworkName", "TransactionHash", "Event" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UniswapLiquidityPositionCashFlows",
                table: "UniswapLiquidityPositionCashFlows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniswapLiquidityPositionCashFlows",
                table: "UniswapLiquidityPositionCashFlows",
                columns: new[] { "PositionId", "NetworkName", "TransactionHash" });
        }
    }
}
