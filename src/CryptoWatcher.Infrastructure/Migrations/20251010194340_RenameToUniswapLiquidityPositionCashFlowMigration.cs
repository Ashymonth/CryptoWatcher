using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameToUniswapLiquidityPositionCashFlowMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiquidityPositionCashFlows_UniswapLiquidityPositions_Positi~",
                table: "LiquidityPositionCashFlows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiquidityPositionCashFlows",
                table: "LiquidityPositionCashFlows");

            migrationBuilder.RenameTable(
                name: "LiquidityPositionCashFlows",
                newName: "UniswapLiquidityPositionCashFlows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniswapLiquidityPositionCashFlows",
                table: "UniswapLiquidityPositionCashFlows",
                columns: new[] { "PositionId", "NetworkName", "TransactionHash" });

            migrationBuilder.AddForeignKey(
                name: "FK_UniswapLiquidityPositionCashFlows_UniswapLiquidityPositions~",
                table: "UniswapLiquidityPositionCashFlows",
                columns: new[] { "PositionId", "NetworkName" },
                principalTable: "UniswapLiquidityPositions",
                principalColumns: new[] { "PositionId", "NetworkName" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniswapLiquidityPositionCashFlows_UniswapLiquidityPositions~",
                table: "UniswapLiquidityPositionCashFlows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UniswapLiquidityPositionCashFlows",
                table: "UniswapLiquidityPositionCashFlows");

            migrationBuilder.RenameTable(
                name: "UniswapLiquidityPositionCashFlows",
                newName: "LiquidityPositionCashFlows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiquidityPositionCashFlows",
                table: "LiquidityPositionCashFlows",
                columns: new[] { "PositionId", "NetworkName", "TransactionHash" });

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidityPositionCashFlows_UniswapLiquidityPositions_Positi~",
                table: "LiquidityPositionCashFlows",
                columns: new[] { "PositionId", "NetworkName" },
                principalTable: "UniswapLiquidityPositions",
                principalColumns: new[] { "PositionId", "NetworkName" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
