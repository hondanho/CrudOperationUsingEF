using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XAutoLeech.Database.EntityFramework;
using XAutoLeech.Database.Model;
using XAutoLeech.Repository;

namespace XAutoLeech
{
    static class Program
    {
        static string ConnectionString = "Data Source=.;Initial Catalog=AutoCrawler;Integrated Security=True;Pooling=False";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Setup DI container
            var serviceProvider = ConfigureServices();

            var form = serviceProvider.GetRequiredService<Main>();
            Application.Run(form);
        }

        static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Add DbContext
            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(ConnectionString);
            });

            services.AddScoped<Repository<Category>>();
            services.AddScoped<Repository<Site>>();
            services.AddScoped<Repository<Post>>();

            // Add your main form
            services.AddScoped<Main>();

            // Build the service provider
            return services.BuildServiceProvider();
        }
    }
}
