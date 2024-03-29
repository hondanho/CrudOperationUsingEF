using Abot2;
using AbotX2.Crawler;
using AbotX2.Parallel;
using AbotX2.Poco;
using Serilog;
using AngleSharp;
using XLeech.Data.EntityFramework;
using System.Net.Http.Headers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using XLeech.Data.Entity;
using AngleSharp.Dom;

namespace XLeech.Core.Service
{

    public class CrawlerService: ICrawlerService
    {
        private AppDbContext _dbContext;

        public CrawlerService(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task CrawlerAsync()
        {
            //Use Serilog to log
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithThreadId()
                .WriteTo.Console(outputTemplate: Constants.LogFormatTemplate)
                .CreateLogger();

            var siteToCrawl = new Uri("https://truyensextv.pro/");

            //Uncomment to demo major features
            //await DemoCrawlerX_PauseResumeStop(siteToCrawl);
            //await DemoCrawlerX_JavascriptRendering(siteToCrawl);
            //await DemoCrawlerX_AutoTuning(siteToCrawl);
            //await DemoCrawlerX_Throttling(siteToCrawl);
            await DemoParallelCrawlerEngine();
        }

        private static async Task DemoCrawlerX_PauseResumeStop(Uri siteToCrawl)
        {
            using (var crawler = new CrawlerX(GetSafeConfig()))
            {
                crawler.PageCrawlCompleted += (sender, args) =>
                {
                    var crawlerPage = args.CrawledPage;
                    Console.WriteLine(crawlerPage.Content.Text);
                    //Check out args.CrawledPage for any info you need
                };
                var crawlTask = crawler.CrawlAsync(siteToCrawl);

                crawler.Pause();    //Suspend all operations

                Thread.Sleep(7000);

                crawler.Resume();   //Resume as if nothing happened

                crawler.Stop(true); //Stop or abort the crawl

                await crawlTask;
            }
        }

        private static async Task DemoCrawlerX_JavascriptRendering(Uri siteToCrawl)
        {
            var pathToPhantomJSExeFolder = @"[YourNugetPackagesLocationAbsolutePath]\PhantomJS.2.1.1\tools\phantomjs]";
            var config = new CrawlConfigurationX
            {
                IsJavascriptRenderingEnabled = true,
                JavascriptRendererPath = pathToPhantomJSExeFolder,
                IsSendingCookiesEnabled = true,
                MaxConcurrentThreads = 1,
                MaxPagesToCrawl = 1,
                JavascriptRenderingWaitTimeInMilliseconds = 3000,
                CrawlTimeoutSeconds = 20
            };

            using (var crawler = new CrawlerX(config))
            {
                crawler.PageCrawlCompleted += (sender, args) =>
                {
                    //JS should be fully rendered here args.CrawledPage.Content.Text
                };

                await crawler.CrawlAsync(siteToCrawl);
            }
        }

        private static async Task DemoCrawlerX_AutoTuning(Uri siteToCrawl)
        {
            var config = GetSafeConfig();
            config.AutoTuning = new AutoTuningConfig
            {
                IsEnabled = true,
                CpuThresholdHigh = 85,
                CpuThresholdMed = 65,
                MinAdjustmentWaitTimeInSecs = 10
            };
            //Optional, configure how aggressively to speed up or down during throttling
            config.Accelerator = new AcceleratorConfig();
            config.Decelerator = new DeceleratorConfig();

            //Now the crawl is able to "AutoTune" itself if the host machine
            //is showing signs of stress.
            using (var crawler = new CrawlerX(config))
            {
                crawler.PageCrawlCompleted += (sender, args) =>
                {
                    //Check out args.CrawledPage for any info you need
                };
                await crawler.CrawlAsync(siteToCrawl);
            }
        }

        private static async Task DemoCrawlerX_Throttling(Uri siteToCrawl)
        {
            var config = GetSafeConfig();
            config.AutoThrottling = new AutoThrottlingConfig
            {
                IsEnabled = true,
                ThresholdHigh = 2,
                ThresholdMed = 2,
                MinAdjustmentWaitTimeInSecs = 10
            };
            //Optional, configure how aggressively to speed up or down during throttling
            config.Accelerator = new AcceleratorConfig();
            config.Decelerator = new DeceleratorConfig();


            using (var crawler = new CrawlerX(config))
            {
                crawler.PageCrawlCompleted += (sender, args) =>
                {
                    //Check out args.CrawledPage for any info you need
                };
                await crawler.CrawlAsync(siteToCrawl);
            }
        }

        private async Task DemoParallelCrawlerEngine()
        {
            var sites = _dbContext.Sites
                        .Include(x => x.Category)
                        .Include(y => y.Post)
                        .ToList();
            List<SiteToCrawl> siteToCrawls = sites.Select(y => new SiteToCrawl
            {
                Uri = new Uri(y.Category.Urls),
                SiteBag = y
            }).ToList();
            var siteToCrawlProvider = new SiteToCrawlProvider();
            siteToCrawlProvider.AddSitesToCrawl(siteToCrawls);
            //siteToCrawlProvider.AddSitesToCrawl(new List<SiteToCrawl>
            //{
            //    new SiteToCrawl{ Uri = new Uri("https://truyensextv.pro/12-nu-than/") },
            //    new SiteToCrawl{ Uri = new Uri("https://truyensextv.pro/chinh-phuc-gai-dep/") },
            //    new SiteToCrawl{ Uri = new Uri("https://truyensextv.pro/con-duong-ba-chu/") },
            //    new SiteToCrawl{ Uri = new Uri("https://truyensextv.pro/co-giao-mon-van/") },
            //    new SiteToCrawl{ Uri = new Uri("https://truyensextv.pro/soi-san-moi-quyen-1/") }
            //});

            var config = GetSafeConfig();
            config.MaxConcurrentSiteCrawls = 3;

            var crawlEngine = new ParallelCrawlerEngine(
                config,
                new ParallelImplementationOverride(config,
                    new ParallelImplementationContainer()
                    {
                        SiteToCrawlProvider = siteToCrawlProvider,
                        WebCrawlerFactory = new WebCrawlerFactory(config)//Same config will be used for every crawler
                    })
                );

            var crawlCounts = new Dictionary<Guid, int>();
            var siteStartingEvents = 0;
            var allSitesCompletedEvents = 0;
            crawlEngine.CrawlerInstanceCreated += (sender, eventArgs) =>
            {
                var crawlId = Guid.NewGuid();
                eventArgs.Crawler.CrawlBag.CrawlId = crawlId;
                eventArgs.Crawler.PageCrawlCompleted += (abotSender, abotEventArgs) =>
                {
                    var crawledPage = abotEventArgs.CrawledPage;
                    var siteBag = eventArgs.SiteToCrawl.SiteBag as SiteConfig;
                    var tagTitle = crawledPage.AngleSharpHtmlDocument.QuerySelector(siteBag.Post.PostTitleSelector);
                    Console.WriteLine("You have the crawled page here in abotEventArgs.CrawledPage..." + abotEventArgs.CrawledPage.Content.Text);
                };
            };
            crawlEngine.SiteCrawlStarting += (sender, args) =>
            {
                Interlocked.Increment(ref siteStartingEvents);
            };
            crawlEngine.SiteCrawlCompleted += (sender, eventArgs) =>
            {
                lock (crawlCounts)
                {
                    crawlCounts.Add(eventArgs.CrawledSite.SiteToCrawl.Id, eventArgs.CrawledSite.CrawlResult.CrawlContext.CrawledCount);
                }
            };
            crawlEngine.AllCrawlsCompleted += (sender, eventArgs) =>
            {
                Interlocked.Increment(ref allSitesCompletedEvents);
            };

            await crawlEngine.StartAsync();
        }

        private static CrawlConfigurationX GetSafeConfig()
        {
            /*The following settings will help not get your ip banned
             by the sites you are trying to crawl. The idea is to crawl
             only 5 pages and wait 2 seconds between http requests
             */
            return new CrawlConfigurationX
            {
                MaxPagesToCrawl = 10,
                MinCrawlDelayPerDomainMilliSeconds = 2000
            };
        }
    }

}
