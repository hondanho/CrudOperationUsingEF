using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XLeech.Data.Migrations
{
    public partial class updatev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Sites",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Sites",
                newName: "Title");
        }
    }
}
