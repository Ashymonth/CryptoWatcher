using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenSymbol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token0Symbol",
                table: "LiquidityPoolPositionSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Token1Symbol",
                table: "LiquidityPoolPositionSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Token0Symbol",
                table: "LiquidityPoolPositions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Token1Symbol",
                table: "LiquidityPoolPositions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token0Symbol",
                table: "LiquidityPoolPositionSnapshots");

            migrationBuilder.DropColumn(
                name: "Token1Symbol",
                table: "LiquidityPoolPositionSnapshots");

            migrationBuilder.DropColumn(
                name: "Token0Symbol",
                table: "LiquidityPoolPositions");

            migrationBuilder.DropColumn(
                name: "Token1Symbol",
                table: "LiquidityPoolPositions");
        }
    }
}
