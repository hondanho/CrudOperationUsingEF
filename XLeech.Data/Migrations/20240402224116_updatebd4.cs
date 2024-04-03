using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XLeech.Data.Migrations
{
    public partial class updatebd4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastCategoryPostURL",
                table: "Sites",
                newName: "CategoryNextPageURL");

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Sites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Sites");

            migrationBuilder.RenameColumn(
                name: "CategoryNextPageURL",
                table: "Sites",
                newName: "LastCategoryPostURL");
        }
    }
}
