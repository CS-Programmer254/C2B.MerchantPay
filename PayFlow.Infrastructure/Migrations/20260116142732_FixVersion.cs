using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Wallets",
                newName: "Balance_Value_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Wallets",
                newName: "Balance_Value_Amount");

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Payments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldRowVersion: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance_Value_Currency",
                table: "Wallets",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "Balance_Value_Amount",
                table: "Wallets",
                newName: "Amount");

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Payments",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 1);
        }
    }
}
