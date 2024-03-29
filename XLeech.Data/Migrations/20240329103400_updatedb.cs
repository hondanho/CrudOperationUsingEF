using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XLeech.Data.Migrations
{
    public partial class updatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUrl",
                table: "Sites",
                newName: "IsPageUrl");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SiteID",
                table: "Posts",
                column: "SiteID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SiteID",
                table: "Categories",
                column: "SiteID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Sites_SiteID",
                table: "Categories",
                column: "SiteID",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Sites_SiteID",
                table: "Posts",
                column: "SiteID",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Sites_SiteID",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Sites_SiteID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SiteID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SiteID",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "IsPageUrl",
                table: "Sites",
                newName: "IsUrl");
        }
    }
}
