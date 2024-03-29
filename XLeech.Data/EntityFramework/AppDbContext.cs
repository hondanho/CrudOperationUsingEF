using Microsoft.EntityFrameworkCore;
using XLeech.Data.Entity;

namespace XLeech.Data.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<SiteConfig> Sites { get; set; }
        public virtual DbSet<CategoryConfig> Categories { get; set; }
        public virtual DbSet<PostConfig> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteConfig>()
                .HasOne(u => u.Category)
                .WithOne(up => up.Site)
                .HasForeignKey<CategoryConfig>(up => up.SiteID);

            modelBuilder.Entity<SiteConfig>()
                .HasOne(u => u.Post)
                .WithOne(up => up.Site)
                .HasForeignKey<PostConfig>(up => up.SiteID);
        }
    }
}
