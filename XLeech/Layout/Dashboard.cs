using Abot2;
using Abot2.Crawler;
using AbotX2.Crawler;
using AbotX2.Parallel;
using AbotX2.Poco;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WordPressPCL.Models;
using XLeech.Core;
using XLeech.Data.Entity;
using XLeech.Data.EntityFramework;
using XLeech.Data.Repository;
using Guid = System.Guid;

namespace XLeech
{
    public partial class Dashboard : UserControl
    {
        private readonly AppDbContext _dbContext;
        private readonly Repository<CategoryConfig> _categoryRepository;

        public Dashboard()
        {
            InitializeComponent();
            if (Main.AppWindow?.AppDbContext != null)
            {
                _dbContext = Main.AppWindow?.AppDbContext;
            }
            if (Main.AppWindow?.CategoryRepository != null)
            {
                _categoryRepository = Main.AppWindow?.CategoryRepository;
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
                       .Where(x => x.ActiveForScheduling)
                       .Include(x => x.Category)
                       .Include(y => y.Post)
                       .ToList();
            await ParallelCrawlerEngineUrls();
        }

        private async Task ParallelCrawlerEngineUrls()
        {
            var sites = _dbContext.Sites
                        .Where(x => x.ActiveForScheduling)
                        .Include(x => x.Category)
                        .Include(y => y.Post)
                        .ToList();
            var siteToCrawlUrls = new List<SiteToCrawl>();

            // site crawle urls
            foreach (var siteConfig in sites.Where(x => x.IsPageUrl))
            {
                string[] postUrls = siteConfig.Category.Urls.Split(new string[] { "\n" }, StringSplitOptions.None);
                siteToCrawlUrls.AddRange(postUrls.Select(x => new SiteToCrawl
                {
                    Uri = new Uri(x),
                    SiteBag = siteConfig
                }).ToList()
                );
            }

            // site crawle category page to urls
            foreach (var siteConfig in sites.Where(x => !x.IsPageUrl))
            {
                using (var crawler = new CrawlerX(GetSafeConfig()))
                {
                    crawler.PageCrawlCompleted += async (sender, args) =>
                    {
                        var wordpressProcessor = new WordpressProcessor(siteConfig);

                        var crawlerPage = args.CrawledPage;
                        var postUrls = await wordpressProcessor.GetPostUrls(crawlerPage.AngleSharpHtmlDocument, siteConfig);
                        siteToCrawlUrls.AddRange(postUrls.Select(x => new SiteToCrawl
                        {
                            Uri = new Uri(x),
                            SiteBag = siteConfig
                        }));
                    };
                    await crawler.CrawlAsync(new Uri(siteConfig.LastCategoryPostURL ?? siteConfig.Category.CategoryPostURL));
                }
            }           

            var siteToCrawlProvider = new SiteToCrawlProvider();
            siteToCrawlProvider.AddSitesToCrawl(siteToCrawlUrls);
            var config = GetSafeConfig();
            config.MaxConcurrentSiteCrawls = 1;

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
                        PageCrawlCompletedUrl(abotSender, abotEventArgs, siteBag);
                    }
                    else
                    {
                        PageCrawlCompletedCategoryPage(abotSender, abotEventArgs, siteBag);
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

            crawlEngine.AllCrawlsCompleted += (sender, eventArgs) =>
            {
                Interlocked.Increment(ref allSitesCompletedEvents);
                ShowLog("All Url Completed");
            };

            await crawlEngine.StartAsync();
        }

        private async void PageCrawlCompletedCategoryPage(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig)
        {
            var crawledPage = abotEventArgs.CrawledPage;
            var wordpressProcessor = new WordpressProcessor(siteConfig);

            var categoryModel = await wordpressProcessor.GetCategory(crawledPage.AngleSharpHtmlDocument, siteConfig);
            var existCategory = await wordpressProcessor.IsExistCategory(categoryModel, siteConfig);
            var category = new Category()
            {
                Id = existCategory?.Id ?? 0
            };
            if (existCategory == null)
            {
                category = await wordpressProcessor.SaveCategory(categoryModel);
            }

            var postModel = await wordpressProcessor.GetPost(crawledPage.AngleSharpHtmlDocument, siteConfig);
            var existPost = await wordpressProcessor.IsExistPost(postModel, siteConfig);
            if (existPost == null)
            {
                await wordpressProcessor.SavePost(postModel, new List<int>() { existCategory != null ? existCategory.Id : category.Id });
            }
        }

        private async void PageCrawlCompletedUrl(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig)
        {
            var crawledPage = abotEventArgs.CrawledPage;
            var wordpressProcessor = new WordpressProcessor(siteConfig);

            var categoryModel = await wordpressProcessor.GetCategory(crawledPage.AngleSharpHtmlDocument, siteConfig);
            var existCategory = await wordpressProcessor.IsExistCategory(categoryModel, siteConfig);
            var category = new Category()
            {
                Id = existCategory?.Id ?? 0
            };
            if (existCategory == null)
            {
                category = await wordpressProcessor.SaveCategory(categoryModel);
            }

            var postModel = await wordpressProcessor.GetPost(crawledPage.AngleSharpHtmlDocument, siteConfig);
            var existPost = await wordpressProcessor.IsExistPost(postModel, siteConfig);
            if (existPost == null)
            {
                await wordpressProcessor.SavePost(postModel, new List<int>() { existCategory != null ? existCategory.Id : category.Id });
            }
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
                MinCrawlDelayPerDomainMilliSeconds = 2000
            };
        }
    }
}