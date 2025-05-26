using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoBeadando.Migrations
{
    /// <inheritdoc />
    public partial class LimitTransactionQickFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "LimitTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "LimitTransactions");
        }
    }
}
