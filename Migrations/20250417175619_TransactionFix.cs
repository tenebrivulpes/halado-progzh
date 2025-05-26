using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoBeadando.Migrations
{
    /// <inheritdoc />
    public partial class TransactionFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsLocked",
                table: "Transactions",
                newName: "Buying");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Buying",
                table: "Transactions",
                newName: "IsLocked");
        }
    }
}
