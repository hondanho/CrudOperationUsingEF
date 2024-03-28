﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XAutoLeech.Database.EntityFramework;

#nullable disable

namespace XAutoLeech.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("XAutoLeech.Database.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CategoryListPageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryListURLSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryMap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryNextPageURLSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryPostURLSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FeaturedImageSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FindAndReplaceRawHTML")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RemoveElementAttributes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SaveFeaturedImages")
                        .HasColumnType("bit");

                    b.Property<int>("SiteID")
                        .HasColumnType("int");

                    b.Property<string>("UnnecessaryElements")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Urls")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("XAutoLeech.Database.Model.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("AddMetaKeywordsAsTag")
                        .HasColumnType("bit");

                    b.Property<string>("CategoryNameSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryNameSeparatorSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FeaturedImageSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FindAndReplaceRawHTML")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PaginatePosts")
                        .HasColumnType("bit");

                    b.Property<string>("PostAuthor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostContentSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostDateSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostExcerptSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostFormat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostNextPageURLSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostSlugSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostTagSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostTitleSelector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RemoveElementAttributes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SaveFeaturedImages")
                        .HasColumnType("bit");

                    b.Property<bool>("SaveMetaDescription")
                        .HasColumnType("bit");

                    b.Property<bool>("SaveMetaKeywords")
                        .HasColumnType("bit");

                    b.Property<int>("SiteID")
                        .HasColumnType("int");

                    b.Property<string>("UnnecessaryElements")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("XAutoLeech.Database.Model.Site", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("ActiveForScheduling")
                        .HasColumnType("bit");

                    b.Property<bool>("CheckDuplicatePostViaContent")
                        .HasColumnType("bit");

                    b.Property<bool>("CheckDuplicatePostViaTitle")
                        .HasColumnType("bit");

                    b.Property<bool>("CheckDuplicatePostViaUrl")
                        .HasColumnType("bit");

                    b.Property<int>("ConnectionTimeout")
                        .HasColumnType("int");

                    b.Property<string>("Cookie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("HTTPUserAgent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsUrl")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastPostCrawl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUrlCollection")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LatestRun")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MaximumPagesCrawlPerCategory")
                        .HasColumnType("int");

                    b.Property<int?>("MaximumPagesCrawlPerPost")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Proxies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProxyRetryLimit")
                        .HasColumnType("int");

                    b.Property<bool>("RandomizeProxies")
                        .HasColumnType("bit");

                    b.Property<int>("TimeInterval")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UseProxy")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });
#pragma warning restore 612, 618
        }
    }
}
