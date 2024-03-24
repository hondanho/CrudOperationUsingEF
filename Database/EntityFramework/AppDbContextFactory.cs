using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAutoLeech.Database.EntityFramework
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private string ConnectionString = "Data Source=.;Initial Catalog=AutoCrawler;Integrated Security=True;Pooling=False";

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
