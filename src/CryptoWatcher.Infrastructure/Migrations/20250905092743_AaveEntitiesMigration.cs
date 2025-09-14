using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AaveEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AavePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Network = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PositionType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtDay = table.Column<DateOnly>(type: "date", nullable: false),
                    ClosedAtDay = table.Column<DateOnly>(type: "date", nullable: true),
                    WalletAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TokenAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
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
                name: "AavePositionSnapshots",
                columns: table => new
                {
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    Token_Symbol = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Token_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false)
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
                name: "IX_AavePositions_WalletAddress",
                table: "AavePositions",
                column: "WalletAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AavePositionSnapshots");

            migrationBuilder.DropTable(
                name: "AavePositions");
        }
    }
}
