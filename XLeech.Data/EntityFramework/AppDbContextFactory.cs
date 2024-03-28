using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace XLeech.Data.EntityFramework
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private string ConnectionString = "Data Source=.;Initial Catalog=XLeech;Integrated Security=True;Pooling=False";

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
