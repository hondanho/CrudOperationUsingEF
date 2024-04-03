using AbotX2.Parallel;
using AbotX2.Poco;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;
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
        private readonly List<ParallelCrawlerEngine> _parallelCrawlerEngine;
        private int siteSuccess = 0;
        private int postSuccess = 0;
        private int postFailed = 0;

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
        }

        public async Task CrawlerAsync()
        {
            var sites = _dbContext.Sites
                       .Where(x => x.ActiveForScheduling && !x.IsDone)
                       .Include(x => x.Category)
                       .Include(y => y.Post)
                       .ToList();
            this.Sitelb.Text = sites?.Count.ToString();
            if (sites.Any())
            {
                foreach (var site in sites)
                {
                    var parallelCrawlerEngineUrl = await ParallelCrawlerEngineUrls(site);
                    _parallelCrawlerEngine.Add(parallelCrawlerEngineUrl);
                }
            }
        }

        private async Task<ParallelCrawlerEngine> ParallelCrawlerEngineUrls(SiteConfig siteConfig)
        {
            var siteToCrawlUrls = new List<SiteToCrawl>();
            var config = GetSafeConfig();

            // get urls from url list
            if (siteConfig.IsPageUrl)
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
            if (!siteConfig.IsPageUrl)
            {
                var categoryNextPageInfo = await _crawlerService.GetCategoryNextPageInfo(siteConfig, config);
                if (categoryNextPageInfo != null && categoryNextPageInfo.PostUrls.Any())
                {
                    siteToCrawlUrls.AddRange(categoryNextPageInfo.PostUrls.Select(x => new SiteToCrawl
                    {
                        Uri = new Uri(x),
                        SiteBag = siteConfig
                    }));
                }
                siteConfig.CategoryNextPageURL = categoryNextPageInfo.CategoryNextPageURL;
            }

            var siteToCrawlProvider = new SiteToCrawlProvider();
            siteToCrawlProvider.AddSitesToCrawl(siteToCrawlUrls);

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

            crawlEngine.CrawlerInstanceCreated += (sender, eventArgs) =>
            {
                var crawlId = Guid.NewGuid();
                eventArgs.Crawler.CrawlBag.CrawlId = crawlId;
                eventArgs.Crawler.PageCrawlCompleted += async (abotSender, abotEventArgs) =>
                {
                    try
                    {
                        var siteBag = eventArgs.SiteToCrawl.SiteBag as SiteConfig;
                        await _crawlerService.PageCrawlCompleted(abotSender, abotEventArgs, siteBag);
                        Interlocked.Increment(ref postSuccess);
                    }
                    catch (Exception ex) {
                        Interlocked.Increment(ref postFailed);
                        ShowLog(string.Format("Exception {0}", ex.Message));
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
                Interlocked.Increment(ref siteSuccess);
                ShowLog(string.Format("All Url Completed: {0} url", crawlCounts.Count()));

                siteConfig.IsDone = string.IsNullOrEmpty(siteConfig.CategoryNextPageURL);
                await _siteConfigRepository.UpdateAsync(siteConfig);

                if (!siteConfig.IsDone)
                {
                    ParallelCrawlerEngineUrls(siteConfig);
                }
            };

            crawlEngine.StartAsync();

            return crawlEngine;
        }

        private void ShowLog(string log)
        {
            if (StatusTb.InvokeRequired)
            {
                StatusTb.Invoke(() => {
                    StatusTb.Text += string.Format("{0} \n", log);
                    StatusTb.SelectionStart = StatusTb.Text.Length;
                    StatusTb.ScrollToCaret();
                });
            }
            else
            {
                StatusTb.Text += string.Format("{0} \n", log);
                StatusTb.SelectionStart = StatusTb.Text.Length;
                StatusTb.ScrollToCaret();
            }
        }

        private static CrawlConfigurationX GetSafeConfig()
        {
            return new CrawlConfigurationX
            {
                MaxPagesToCrawl = 1,
                MinCrawlDelayPerDomainMilliSeconds = 5000,
                //MaxConcurrentSiteCrawls = 5,
                //HttpRequestTimeoutInSeconds= 60,
                //MaxConcurrentThreads = 5,
            };
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            CrawlerAsync();
        }

        private async void ReCrawleBtn_Click(object sender, EventArgs e)
        {
            var sites = _dbContext.Sites
                       .Where(x => x.ActiveForScheduling && !x.IsDone)
                       .Include(x => x.Category)
                       .Include(y => y.Post)
                       .ToList();
            if (sites.Any())
            {
                foreach (var site in sites)
                {
                    site.IsDone = false;
                    site.CategoryNextPageURL = null;
                    await _siteConfigRepository.UpdateAsync(site);
                }
            }

            CrawlerAsync();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            foreach (var parallelCrawlerEngine in _parallelCrawlerEngine)
            {
                parallelCrawlerEngine.Stop();
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void Sitelb_Click(object sender, EventArgs e)
        {

        }
    }
}