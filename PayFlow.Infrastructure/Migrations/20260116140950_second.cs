using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "Wallets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Merchants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Customers",
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
                name: "Currency",
                table: "Wallets",
                newName: "Balance_Value_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Wallets",
                newName: "Balance_Value_Amount");

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Wallets",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Merchants",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Customers",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 1);
        }
    }
}
