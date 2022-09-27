using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class Profiletablemodify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LaseName",
                table: "Profile",
                newName: "Remarks");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Profile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Profile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Profile");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "Profile",
                newName: "LaseName");
        }
    }
}
