using XLeech.Data.Repository;
using XLeech.Data.Entity;
using XLeech.Data.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using XLeech.Core.Service;

namespace XLeech
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize Configuration
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // Setup DI container
            var serviceProvider = ConfigureServices(configuration);

            var form = serviceProvider.GetRequiredService<Main>();
            Application.Run(form);
        }

        static IServiceProvider ConfigureServices(IConfiguration configuration)
        {
            // Access Connection String
            string connectionString = configuration.GetConnectionString("XLeech");
            var services = new ServiceCollection();

            // Add DbContext
            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<Repository<CategoryConfig>>();
            services.AddScoped<Repository<Data.Entity.SiteConfig>>();
            services.AddScoped<Repository<PostConfig>>();
            services.AddScoped<CrawlerService>();

            // Add your main form
            services.AddScoped<Main>();

            // Build the service provider
            return services.BuildServiceProvider();
        }
    }
}