using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XLeech.Data.Migrations
{
    public partial class updatedb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryPostURL",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryPostURL",
                table: "Categories");
        }
    }
}
