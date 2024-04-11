using XLeech.Data.Repository;
using XLeech.Data.Entity;
using XLeech.Data.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using XLeech.Core.Service;
using AbotX2.Parallel;
using XLeech.Core;
using MathNet.Numerics;
using AbotX2.Poco;

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

            services.AddHttpClient();
            services.AddScoped<IChatGPTService, ChatGPTService>();
            services.AddSingleton<ParallelCrawlerEngine>(x => {
                var config = new CrawlConfigurationX
                {
                    MinSiteToCrawlRequestDelayInSecs = 1000,
                    SitesToCrawlBatchSizePerRequest = 1000,
                    MinCrawlDelayPerDomainMilliSeconds = 10000,
                    MaxConcurrentSiteCrawls = 1,
                    MaxPagesToCrawl = 1,
                    IsIgnoreRobotsDotTextIfRootDisallowedEnabled = true,
                    IsSendingCookiesEnabled = true
                };
                var siteToCrawlProvider = new AlwaysOnSiteToCrawlProvider();
                var parallelCrawlerEngine = new ParallelCrawlerEngine(
                    config,
                    new ParallelImplementationOverride(config,
                            new ParallelImplementationContainer()
                            {
                                SiteToCrawlProvider = siteToCrawlProvider,
                                WebCrawlerFactory = new WebCrawlerFactory(config)
                            }
                        )
                    );
                parallelCrawlerEngine.Impls.ImplementationBag.CategoryCrawled = 0;
                parallelCrawlerEngine.Impls.ImplementationBag.PostSuccess = 0;
                parallelCrawlerEngine.Impls.ImplementationBag.PostFailed = 0;
                parallelCrawlerEngine.StartAsync();
                return parallelCrawlerEngine;
            });
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<Repository<CategoryConfig>>();
            services.AddScoped<Repository<Data.Entity.SiteConfig>>();
            services.AddScoped<Repository<PostConfig>>();
            services.AddScoped<ICrawlerService, CrawlerService>();

            // Add your main form
            services.AddScoped<Main>();

            // Build the service provider
            return services.BuildServiceProvider();
        }
    }
}