using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XAutoLeech.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteID = table.Column<int>(type: "int", nullable: false),
                    CategoryListPageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryListURLSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryMap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryPostURLSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryNextPageURLSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaveFeaturedImages = table.Column<bool>(type: "bit", nullable: false),
                    FeaturedImageSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FindDndReplaceRawHTML = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemoveElementAttributes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnnecessaryElements = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteID = table.Column<int>(type: "int", nullable: false),
                    PostTitleSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostExcerptSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostContentSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostTagSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostSlugSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryNameSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryNameSeparatorSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDateSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaveMetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddMetaKeywordsAsTag = table.Column<bool>(type: "bit", nullable: false),
                    SaveMetaDescription = table.Column<bool>(type: "bit", nullable: false),
                    FeaturedImageSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaginatePosts = table.Column<bool>(type: "bit", nullable: false),
                    PostNextPageURLSelector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FindDndReplaceRawHTML = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemoveElementAttributes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnnecessaryElements = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveForScheduling = table.Column<bool>(type: "bit", nullable: false),
                    CheckDuplicatePostViaUrl = table.Column<bool>(type: "bit", nullable: false),
                    CheckDuplicatePostViaTitle = table.Column<bool>(type: "bit", nullable: false),
                    CheckDuplicatePostViaContent = table.Column<bool>(type: "bit", nullable: false),
                    MaximumPagesCrawlPerCategory = table.Column<int>(type: "int", nullable: true),
                    MaximumPagesCrawlPerPost = table.Column<int>(type: "int", nullable: true),
                    AllowComment = table.Column<bool>(type: "bit", nullable: false),
                    PostStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostAuthor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HTTPUserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionTimeout = table.Column<int>(type: "int", nullable: false),
                    UseProxy = table.Column<bool>(type: "bit", nullable: false),
                    Proxies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProxyRetryLimit = table.Column<int>(type: "int", nullable: false),
                    RandomizeProxies = table.Column<bool>(type: "bit", nullable: false),
                    TimeInterval = table.Column<int>(type: "int", nullable: false),
                    LatestRun = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUrlCollection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastPostCrawl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
