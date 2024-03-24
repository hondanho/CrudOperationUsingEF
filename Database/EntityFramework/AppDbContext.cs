using Microsoft.EntityFrameworkCore;
using XAutoLeech.Database.Model;

namespace XAutoLeech.Database.EntityFramework
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
    }
}
