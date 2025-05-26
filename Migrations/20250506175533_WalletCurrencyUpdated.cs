using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoBeadando.Migrations
{
    /// <inheritdoc />
    public partial class WalletCurrencyUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CryptoId",
                table: "WalletCurrencies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CryptoId",
                table: "WalletCurrencies");
        }
    }
}
