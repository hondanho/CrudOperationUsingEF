using CrudOperation.Model;
using Microsoft.EntityFrameworkCore;

namespace CrudOperation.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AutoCrawler;Integrated Security=True;Pooling=False");
        }

        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
    }
}
