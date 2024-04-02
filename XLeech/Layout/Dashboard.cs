using Abot2;
using AbotX2.Parallel;
using AbotX2.Poco;
using Microsoft.EntityFrameworkCore;
using Serilog;
using XLeech.Core.Service;
using XLeech.Data.Entity;
using XLeech.Data.EntityFramework;
using XLeech.Data.Repository;
using Guid = System.Guid;

namespace XLeech
{
    public partial class Dashboard : UserControl
    {
        private readonly AppDbContext _dbContext;
        private readonly Repository<SiteConfig> _siteConfigRepository;
        private readonly CrawlerService _crawlerService;

        public Dashboard()
        {
            InitializeComponent();
            if (Main.AppWindow?.AppDbContext != null)
            {
                _dbContext = Main.AppWindow?.AppDbContext;
            }
            if (Main.AppWindow?.SiteConfigRepository != null)
            {
                _siteConfigRepository = Main.AppWindow?.SiteConfigRepository;
            }
            if (Main.AppWindow?.CrawlerService != null)
            {
                _crawlerService = Main.AppWindow?.CrawlerService;
            }

            CrawlerAsync();
        }

        public async Task CrawlerAsync()
        {
            //Use Serilog to log
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithThreadId()
                .WriteTo.Console(outputTemplate: Constants.LogFormatTemplate)
                .CreateLogger();

            var sites = _dbContext.Sites
                       .Where(x => x.ActiveForScheduling && !x.IsDoneCrawle)
                       .Include(x => x.Category)
                       .Include(y => y.Post)
                       .ToList();
            await ParallelCrawlerEngineUrls(sites);
        }

        private async Task ParallelCrawlerEngineUrls(List<SiteConfig> siteConfigs)
        {
            var siteToCrawlUrls = new List<SiteToCrawl>();
            var siteConfigOrigins = new List<SiteConfig>();

            // get urls from url list
            foreach (var siteConfig in siteConfigs.Where(x => x.IsPageUrl))
            {
                string[] postUrls = siteConfig.Category.Urls.Split(new string[] { "\n" }, StringSplitOptions.None);
                siteToCrawlUrls.AddRange(postUrls.Select(x => new SiteToCrawl
                {
                    Uri = new Uri(x),
                    SiteBag = siteConfig
                }).ToList()
                );
            }

            // get urls from category page
            foreach (var siteConfig in siteConfigs.Where(x => !x.IsPageUrl))
            {
                var categoryNextPageInfo = await _crawlerService.GetCategoryNextPageInfo(siteConfig, GetSafeConfig());
                if (categoryNextPageInfo != null && categoryNextPageInfo.PostUrls.Any())
                {
                    siteToCrawlUrls.AddRange(categoryNextPageInfo.PostUrls.Select(x => new SiteToCrawl
                    {
                        Uri = new Uri(x),
                        SiteBag = siteConfig
                    }));
                }
                siteConfig.CategoryNextPageURL = categoryNextPageInfo.CategoryNextPageURL;
                siteConfigOrigins.Add(siteConfig);
            }

            var siteToCrawlProvider = new SiteToCrawlProvider();
            siteToCrawlProvider.AddSitesToCrawl(siteToCrawlUrls);
            var config = GetSafeConfig();

            var crawlEngine = new ParallelCrawlerEngine(
                config,
                new ParallelImplementationOverride(config,
                    new ParallelImplementationContainer()
                    {
                        SiteToCrawlProvider = siteToCrawlProvider,
                        WebCrawlerFactory = new WebCrawlerFactory(config) //Same config will be used for every crawler
                    })
                );

            var crawlCounts = new Dictionary<Guid, int>();
            var siteStartingEvents = 0;
            var allSitesCompletedEvents = 0;

            crawlEngine.CrawlerInstanceCreated += (sender, eventArgs) =>
            {
                var crawlId = Guid.NewGuid();
                eventArgs.Crawler.CrawlBag.CrawlId = crawlId;
                eventArgs.Crawler.PageCrawlCompleted += async (abotSender, abotEventArgs) =>
                {
                    var siteBag = eventArgs.SiteToCrawl.SiteBag as SiteConfig;
                    if (siteBag.IsPageUrl)
                    {
                        _crawlerService.PageCrawlCompletedUrl(abotSender, abotEventArgs, siteBag);
                    }
                    else
                    {
                        _crawlerService.PageCrawlCompletedCategoryPage(abotSender, abotEventArgs, siteBag);
                    }
                };
            };

            crawlEngine.SiteCrawlStarting += (sender, args) =>
            {
                Interlocked.Increment(ref siteStartingEvents);
                ShowLog(string.Format("{0} started", args.SiteToCrawl.Uri));
            };

            crawlEngine.SiteCrawlCompleted += (sender, args) =>
            {
                lock (crawlCounts)
                {
                    crawlCounts.Add(args.CrawledSite.SiteToCrawl.Id, args.CrawledSite.CrawlResult.CrawlContext.CrawledCount);
                }
                ShowLog(string.Format("{0} Completed", args.CrawledSite.SiteToCrawl.Uri));
            };

            crawlEngine.AllCrawlsCompleted += async (sender, eventArgs) =>
            {
                Interlocked.Increment(ref allSitesCompletedEvents);
                ShowLog(string.Format("All Url Completed: {0} url, ", crawlCounts.Count()));
                foreach (var siteConfig in siteConfigOrigins)
                {
                    await _siteConfigRepository.UpdateAsync(siteConfig);
                }

                var siteConfigNexts = siteConfigOrigins.Where(x => !string.IsNullOrEmpty(x.CategoryNextPageURL)).ToList();
                if (siteConfigNexts.Any())
                {
                    ParallelCrawlerEngineUrls(siteConfigNexts);
                }

                var siteConfigsCrawleDone = siteConfigOrigins.Where(x => string.IsNullOrEmpty(x.CategoryNextPageURL)).ToList();
                foreach (var siteConfig in siteConfigsCrawleDone)
                {
                    siteConfig.IsDoneCrawle = true;
                    await _siteConfigRepository.UpdateAsync(siteConfig);
                }
            };

            await crawlEngine.StartAsync();
        }

        private void ShowLog(string log)
        {
            if (StatusTb.InvokeRequired)
            {
                StatusTb.Invoke(() => StatusTb.Text += string.Format("{0} \n", log));
            }
            else
            {
                StatusTb.Text += string.Format("{0} \n", log);
            }
        }

        private static CrawlConfigurationX GetSafeConfig()
        {
            return new CrawlConfigurationX
            {
                MaxPagesToCrawl = 1,
                MinCrawlDelayPerDomainMilliSeconds = 5000,
                MaxConcurrentSiteCrawls = 1
            };
        }
    }
}