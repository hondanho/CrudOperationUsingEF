using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XAutoLeech.Migrations
{
    public partial class update_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowComment",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "PostAuthor",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "PostStatus",
                table: "Sites");

            migrationBuilder.RenameColumn(
                name: "PostType",
                table: "Sites",
                newName: "Cookie");

            migrationBuilder.AddColumn<string>(
                name: "PostAuthor",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostStatus",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostType",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostAuthor",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostStatus",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostType",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Cookie",
                table: "Sites",
                newName: "PostType");

            migrationBuilder.AddColumn<bool>(
                name: "AllowComment",
                table: "Sites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PostAuthor",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostStatus",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
