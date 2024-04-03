using AbotX2.Parallel;
using AbotX2.Poco;
using Microsoft.EntityFrameworkCore;
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
        private int categoryCrawled = 0;
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

        private async Task CrawlerAsync()
        {
            var sites = _dbContext.Sites
                       .Where(x => x.ActiveForScheduling && !x.IsDone)
                       .Include(x => x.Category)
                       .Include(y => y.Post)
                       .ToList();
            this.AllSiteLb.Text = sites?.Count.ToString();
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
            var categoryPageURLCrawle = _crawlerService.GetCategoryPageURLCrawle(siteConfig);

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
                    catch (Exception ex)
                    {
                        Interlocked.Increment(ref postFailed);
                        ShowLabel(this.PostFailedLb, postFailed.ToString());
                        LogPost(string.Format("{0} Exception {1}", eventArgs.SiteToCrawl.Uri, ex.Message));
                    }
                };
            };

            crawlEngine.SiteCrawlStarting += (sender, args) =>
            {
                LogPost(string.Format("{0} Started", args.SiteToCrawl.Uri));
            };

            crawlEngine.SiteCrawlCompleted += (sender, args) =>
            {
                Interlocked.Increment(ref postSuccess);
                ShowLabel(this.PostSuccessLb, postSuccess.ToString());
                LogPost(string.Format("{0} Completed", args.CrawledSite.SiteToCrawl.Uri));
            };

            crawlEngine.AllCrawlsCompleted += async (sender, eventArgs) =>
            {
                Interlocked.Increment(ref categoryCrawled);
                ShowLabel(this.CategoryCrawledLb, categoryCrawled.ToString());
                LogSite(string.Format("{0} Completed", categoryPageURLCrawle));

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

        #region Log
        private void ShowLabel(Label label, string log)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(() =>
                {
                    label.Text += string.Format("{0} {1}\n", DateTime.Now, log);
                });
            }
            else
            {
                label.Text += string.Format("{0} {1}\n", DateTime.Now, log);
            }
        }

        private void LogSite(string log)
        {
            ShowLog(this.LogSiteTb, log);
        }

        private void LogPost(string log)
        {
            ShowLog(this.LogPostTb, log);
        }

        private void ShowLog(RichTextBox richTextBox, string log)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(() =>
                {
                    richTextBox.Text += string.Format("{0} {1}\n", DateTime.Now, log);
                    richTextBox.SelectionStart = richTextBox.Text.Length;
                    richTextBox.ScrollToCaret();
                });
            }
            else
            {
                richTextBox.Text += string.Format("{0} {1}\n", DateTime.Now, log);
                richTextBox.SelectionStart = richTextBox.Text.Length;
                richTextBox.ScrollToCaret();
            }
        }

        #endregion
    }
}