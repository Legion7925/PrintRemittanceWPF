using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintRemittance.Core.Migrations
{
    /// <inheritdoc />
    public partial class added_PlateNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemittanceNumber",
                table: "Documents",
                newName: "PlateNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlateNumber",
                table: "Documents",
                newName: "RemittanceNumber");
        }
    }
}
