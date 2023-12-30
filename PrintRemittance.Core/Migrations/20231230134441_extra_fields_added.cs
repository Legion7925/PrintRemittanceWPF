using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintRemittance.Core.Migrations
{
    /// <inheritdoc />
    public partial class extra_fields_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlateNumber",
                table: "Documents",
                newName: "RemittanceNumber");

            migrationBuilder.AddColumn<string>(
                name: "CarName",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FactoryName",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PrintNumber",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarName",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FactoryName",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "PrintNumber",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "RemittanceNumber",
                table: "Documents",
                newName: "PlateNumber");
        }
    }
}
