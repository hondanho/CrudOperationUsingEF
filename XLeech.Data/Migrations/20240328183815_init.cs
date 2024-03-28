using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XLeech.Data.Migrations
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
                    CategoryListPageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryListURLSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryPostURLSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryNextPageURLSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Urls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryMap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaveFeaturedImages = table.Column<bool>(type: "bit", nullable: false),
                    FeaturedImageSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FindAndReplaceRawHTML = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoveElementAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnnecessaryElements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    PostFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostAuthor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostTitleSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostExcerptSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostContentSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostTagSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostSlugSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryNameSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryNameSeparatorSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostDateSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaveMetaKeywords = table.Column<bool>(type: "bit", nullable: false),
                    AddMetaKeywordsAsTag = table.Column<bool>(type: "bit", nullable: false),
                    SaveMetaDescription = table.Column<bool>(type: "bit", nullable: false),
                    SaveFeaturedImages = table.Column<bool>(type: "bit", nullable: false),
                    FeaturedImageSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaginatePosts = table.Column<bool>(type: "bit", nullable: false),
                    PostNextPageURLSelector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FindAndReplaceRawHTML = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoveElementAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnnecessaryElements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUrl = table.Column<bool>(type: "bit", nullable: false),
                    ActiveForScheduling = table.Column<bool>(type: "bit", nullable: false),
                    CheckDuplicatePostViaUrl = table.Column<bool>(type: "bit", nullable: false),
                    CheckDuplicatePostViaTitle = table.Column<bool>(type: "bit", nullable: false),
                    CheckDuplicatePostViaContent = table.Column<bool>(type: "bit", nullable: false),
                    MaximumPagesCrawlPerCategory = table.Column<int>(type: "int", nullable: true),
                    MaximumPagesCrawlPerPost = table.Column<int>(type: "int", nullable: true),
                    HTTPUserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cookie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectionTimeout = table.Column<int>(type: "int", nullable: false),
                    UseProxy = table.Column<bool>(type: "bit", nullable: false),
                    Proxies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProxyRetryLimit = table.Column<int>(type: "int", nullable: false),
                    RandomizeProxies = table.Column<bool>(type: "bit", nullable: false),
                    TimeInterval = table.Column<int>(type: "int", nullable: false),
                    LatestRun = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUrlCollection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastPostCrawl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
