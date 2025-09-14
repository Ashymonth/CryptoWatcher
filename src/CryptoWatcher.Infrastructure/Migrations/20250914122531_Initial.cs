using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Networks",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RpcUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    NftManagerAddress = table.Column<string>(type: "character varying(266)", maxLength: 266, nullable: false),
                    PoolFactoryAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MultiCallAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ProtocolVersion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Networks", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Address = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Address);
                });

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
                    TokenAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PreviousScaledAmount = table.Column<decimal>(type: "numeric", nullable: true)
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
                name: "HyperliquidVaultPositions",
                columns: table => new
                {
                    VaultAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    WalletAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidVaultPositions", x => new { x.VaultAddress, x.WalletAddress });
                    table.ForeignKey(
                        name: "FK_HyperliquidVaultPositions_Wallets_WalletAddress",
                        column: x => x.WalletAddress,
                        principalTable: "Wallets",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoolPositions",
                columns: table => new
                {
                    PositionId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    NetworkName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Token0_Symbol = table.Column<string>(type: "text", nullable: false),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    Token1_Symbol = table.Column<string>(type: "text", nullable: false),
                    Token1_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token1_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    WalletAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolPositions", x => new { x.PositionId, x.NetworkName });
                    table.ForeignKey(
                        name: "FK_PoolPositions_Networks_NetworkName",
                        column: x => x.NetworkName,
                        principalTable: "Networks",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PoolPositions_Wallets_WalletAddress",
                        column: x => x.WalletAddress,
                        principalTable: "Wallets",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AavePositionEvent",
                columns: table => new
                {
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    Token_Symbol = table.Column<string>(type: "text", nullable: false),
                    Token_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AavePositionEvent", x => new { x.PositionId, x.Date, x.EventType });
                    table.ForeignKey(
                        name: "FK_AavePositionEvent_AavePositions_PositionId",
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

            migrationBuilder.CreateTable(
                name: "HyperliquidVaultEvents",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VaultAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    WalletAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Usd = table.Column<decimal>(type: "numeric", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidVaultEvents", x => new { x.VaultAddress, x.WalletAddress, x.Date });
                    table.ForeignKey(
                        name: "FK_HyperliquidVaultEvents_HyperliquidVaultPositions_VaultAddre~",
                        columns: x => new { x.VaultAddress, x.WalletAddress },
                        principalTable: "HyperliquidVaultPositions",
                        principalColumns: new[] { "VaultAddress", "WalletAddress" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HyperliquidVaultEvents_Wallets_WalletAddress",
                        column: x => x.WalletAddress,
                        principalTable: "Wallets",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HyperliquidVaultPositionSnapshots",
                columns: table => new
                {
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    VaultAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    WalletAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperliquidVaultPositionSnapshots", x => new { x.VaultAddress, x.WalletAddress, x.Day });
                    table.ForeignKey(
                        name: "FK_HyperliquidVaultPositionSnapshots_HyperliquidVaultPositions~",
                        columns: x => new { x.VaultAddress, x.WalletAddress },
                        principalTable: "HyperliquidVaultPositions",
                        principalColumns: new[] { "VaultAddress", "WalletAddress" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HyperliquidVaultPositionSnapshots_Wallets_WalletAddress",
                        column: x => x.WalletAddress,
                        principalTable: "Wallets",
                        principalColumn: "Address",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoolPositionSnapshots",
                columns: table => new
                {
                    PoolPositionId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    NetworkName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Day = table.Column<DateOnly>(type: "date", nullable: false),
                    IsInRange = table.Column<bool>(type: "boolean", nullable: false),
                    Token0_Symbol = table.Column<string>(type: "text", nullable: false),
                    Token0_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_FeeAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token0_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false),
                    Token1_Symbol = table.Column<string>(type: "text", nullable: false),
                    Token1_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token1_FeeAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Token1_PriceInUsd = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolPositionSnapshots", x => new { x.PoolPositionId, x.NetworkName, x.Day });
                    table.ForeignKey(
                        name: "FK_PoolPositionSnapshots_PoolPositions_PoolPositionId_NetworkN~",
                        columns: x => new { x.PoolPositionId, x.NetworkName },
                        principalTable: "PoolPositions",
                        principalColumns: new[] { "PositionId", "NetworkName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AavePositions_WalletAddress",
                table: "AavePositions",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HyperliquidVaultEvents_WalletAddress",
                table: "HyperliquidVaultEvents",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HyperliquidVaultPositions_WalletAddress",
                table: "HyperliquidVaultPositions",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HyperliquidVaultPositionSnapshots_WalletAddress",
                table: "HyperliquidVaultPositionSnapshots",
                column: "WalletAddress");

            migrationBuilder.CreateIndex(
                name: "IX_PoolPositions_NetworkName",
                table: "PoolPositions",
                column: "NetworkName");

            migrationBuilder.CreateIndex(
                name: "IX_PoolPositions_WalletAddress",
                table: "PoolPositions",
                column: "WalletAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AavePositionEvent");

            migrationBuilder.DropTable(
                name: "AavePositionSnapshots");

            migrationBuilder.DropTable(
                name: "HyperliquidVaultEvents");

            migrationBuilder.DropTable(
                name: "HyperliquidVaultPositionSnapshots");

            migrationBuilder.DropTable(
                name: "PoolPositionSnapshots");

            migrationBuilder.DropTable(
                name: "AavePositions");

            migrationBuilder.DropTable(
                name: "HyperliquidVaultPositions");

            migrationBuilder.DropTable(
                name: "PoolPositions");

            migrationBuilder.DropTable(
                name: "Networks");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
