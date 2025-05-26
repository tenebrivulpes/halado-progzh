using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoBeadando.Migrations
{
    /// <inheritdoc />
    public partial class WalletCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Wallets_WalletEntityId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_WalletEntityId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "WalletEntityId",
                table: "Currencies");

            migrationBuilder.CreateTable(
                name: "WalletCurrencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    WalletEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletCurrencies_Wallets_WalletEntityId",
                        column: x => x.WalletEntityId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletCurrencies_WalletEntityId",
                table: "WalletCurrencies",
                column: "WalletEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletCurrencies");

            migrationBuilder.AddColumn<float>(
                name: "Quantity",
                table: "Currencies",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "WalletEntityId",
                table: "Currencies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_WalletEntityId",
                table: "Currencies",
                column: "WalletEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Wallets_WalletEntityId",
                table: "Currencies",
                column: "WalletEntityId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }
    }
}
