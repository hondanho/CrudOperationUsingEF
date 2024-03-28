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

        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Site>()
                .HasOne(u => u.Category)
                .WithOne(up => up.Site)
                .HasForeignKey<Category>(up => up.SiteID);

            modelBuilder.Entity<Site>()
                .HasOne(u => u.Post)
                .WithOne(up => up.Site)
                .HasForeignKey<Post>(up => up.SiteID);
        }
    }
}
