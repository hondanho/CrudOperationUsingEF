using Abot2.Core;
using Abot2.Poco;
using AbotX2.Core;
using AbotX2.Crawler;
using AbotX2.Parallel;
using AbotX2.Poco;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Text;
using XLeech.Core.Extensions;
using XLeech.Core.Model;
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
        private readonly ICrawlerService _crawlerService;
        private readonly List<ParallelCrawlerEngine> _parallelCrawlerEngine = new List<ParallelCrawlerEngine>();
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
                MinCrawlDelayPerDomainMilliSeconds = 10000,
                MaxConcurrentSiteCrawls = 3,
<<<<<<< HEAD
                IsSendingCookiesEnabled = true
=======
                ConfigurationExtensions= {}
>>>>>>> a7fd29b38b532188644abebf35c27dee4ec731b6
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
                    ParallelCrawlerEngineUrls(site);
                }
            }
            else
            {
                MessageBox.Show("No site crawle");
            }
        }

        private async Task<ParallelCrawlerEngine> ParallelCrawlerEngineUrls(SiteConfig siteConfig)
        {
            var siteToCrawlUrls = new List<SiteToCrawl>();
            var config = GetSafeConfig();
            var categoryPageURLCrawle = _crawlerService.GetCategoryPageURLCrawle(siteConfig);

            try
            {
                // get urls from url list
                if (siteConfig.IsPageUrl)
                {
                    string[] postUrls = siteConfig.Category.Urls?.ToListString();
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

                var siteToCrawlProvider = new AlwaysOnSiteToCrawlProvider();
                siteToCrawlProvider.AddSitesToCrawl(siteToCrawlUrls);
                var crawlEngine = new ParallelCrawlerEngine(
                    config,
                    new ParallelImplementationOverride(config,
                            new ParallelImplementationContainer()
                            {
                                SiteToCrawlProvider = siteToCrawlProvider,
                                
                                WebCrawlerFactory = new WebCrawlerFactory(config) //Same config will be used for every crawler
                            }
                        )
                    );

                crawlEngine.CrawlerInstanceCreated += (sender, eventArgs) =>
                {
                    var crawlId = Guid.NewGuid();
                    eventArgs.Crawler.CrawlBag.CrawlId = crawlId;
                    var impls = new ImplementationOverride(config, new ImplementationContainer {
                        PageRequester = new YourCustomPageRequester(config, null)
                    });
                    eventArgs = new CrawlerInstanceCreatedArgs(eventArgs.SiteToCrawl, new CrawlerX(config, impls)); 
                    eventArgs.Crawler.PageCrawlCompleted += async (abotSender, abotEventArgs) =>
                    {
                        try
                        {
                            var siteBag = eventArgs.SiteToCrawl.SiteBag as SiteConfig;
                            var crawlerResult = await _crawlerService.PageCrawlCompleted(abotSender, abotEventArgs, siteBag);


                            Interlocked.Increment(ref postSuccess);
                            LogLabel(this.PostSuccessLb, postSuccess.ToString());
                            LogPost(string.Format("{0} Completed, {1}", abotEventArgs.CrawledPage.Uri, crawlerResult.IsSavePost ? "Save" : "Skiped"));
                        }
                        catch (Exception ex)
                        {
                            Interlocked.Increment(ref postFailed);
                            LogLabel(this.PostFailedLb, postFailed.ToString());
                            LogPost(string.Format("{0} Exception {1}", eventArgs.SiteToCrawl.Uri, ex.Message));
                        }
                    };
                };

                crawlEngine.SiteCrawlStarting += (sender, args) =>
                {
                };

                crawlEngine.SiteCrawlCompleted += (sender, args) =>
                {
                };

                crawlEngine.AllCrawlsCompleted += async (sender, eventArgs) =>
                {
                    //var parallelCrawlerEngine = _parallelCrawlerEngine.Where(x => x.)

                    Interlocked.Increment(ref categoryCrawled);
                    LogLabel(this.CategoryCrawledLb, categoryCrawled.ToString());
                    LogSite(string.Format("{0} Completed", categoryPageURLCrawle));

                    siteConfig.IsDone = string.IsNullOrEmpty(siteConfig.CategoryNextPageURL);
                    await _siteConfigRepository.UpdateAsync(siteConfig);
                    if (!siteConfig.IsDone)
                    {
                        ParallelCrawlerEngineUrls(siteConfig);
                    }
                    

                    if (categoryCrawled < 2)
                    {
                        siteToCrawlProvider.AddSitesToCrawl(siteToCrawlUrls);
                        crawlEngine.Pause();
                        await crawlEngine.StartAsync();
                    } 
                };

                await crawlEngine.StartAsync();
                _parallelCrawlerEngine.Add(crawlEngine);
                return crawlEngine;
            }
            catch (Exception ex)
            {
                LogSite(string.Format("{0} Exception {1}", categoryPageURLCrawle, ex.Message));
            }

            return new ParallelCrawlerEngine(config);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            CrawlerAsync();
        }

        private async void ReCrawleBtn_Click(object sender, EventArgs e)
        {
            var sites = _dbContext.Sites
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
                if (parallelCrawlerEngine.Impls.SiteToCrawlProvider.IsComplete)
                {
                    _parallelCrawlerEngine.Remove(parallelCrawlerEngine);
                }
                else
                {
                    parallelCrawlerEngine.Stop();
                }
            }
        }

        

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            foreach (var parallelCrawlerEngine in _parallelCrawlerEngine)
            {
                if (parallelCrawlerEngine.Impls.SiteToCrawlProvider.IsComplete)
                {
                    _parallelCrawlerEngine.Remove(parallelCrawlerEngine);
                }
                else
                {
                    parallelCrawlerEngine.Pause();
                }
            }
        }

        #region Log
        private void LogLabel(Label label, string log)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(() =>
                {
                    label.Text = log;
                });
            }
            else
            {
                label.Text = log;
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
                    richTextBox.Text += string.Format("{0} {1}\n", DateTime.Now.ToString(Constants.FormatDateShowLog), log);
                    richTextBox.SelectionStart = richTextBox.Text.Length;
                    richTextBox.ScrollToCaret();
                });
            }
            else
            {
                richTextBox.Text += string.Format("{0} {1}\n", DateTime.Now.ToString(Constants.FormatDateShowLog), log);
                richTextBox.SelectionStart = richTextBox.Text.Length;
                richTextBox.ScrollToCaret();
            }
        }

        #endregion
    }

    public class YourCustomPageRequester : PageRequester
    {
        private readonly CrawlConfiguration _config;
       public YourCustomPageRequester(CrawlConfiguration config, IWebContentExtractor contentExtractor) : base(config, contentExtractor) {
            _config = config;
        }
        protected virtual HttpClient BuildHttpClient(HttpClientHandler clientHandler)
        {
            // authenticated proxy info
            List<string> proxies = new List<string>
            {
                "http://129.151.91.248:80",
                "http://18.169.189.181:80",
                // ...
                "http://212.76.110.242:80"
            };

            // extract a random proxy from the list
            Random random = new Random();
            int index = random.Next(proxies.Count);
            string proxyURL = proxies[index];

            //WebProxy webProxy = new WebProxy
            //{
            //    Address = new Uri(proxyURL),
            //    // specify the proxy credentials
            //    Credentials = new NetworkCredential(
            //        userName: proxyUsername,
            //        password: proxyPassword
            //  )
            //};


            //clientHandler.Proxy = new WebProxy(_config.ConfigurationExtensions["MyProxy1"]);
            clientHandler.Proxy = new WebProxy(proxyURL);
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", _config.UserAgentString);
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            if (_config.HttpRequestTimeoutInSeconds > 0)
            {
                httpClient.Timeout = TimeSpan.FromSeconds(_config.HttpRequestTimeoutInSeconds);
            }

            if (_config.IsAlwaysLogin)
            {
                string text = Convert.ToBase64String(Encoding.ASCII.GetBytes(_config.LoginUser + ":" + _config.LoginPassword));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + text);
            }

            return httpClient;
        }
    }
}