using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using AbotX2.Parallel;
using AbotX2.Poco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Diagnostics.Tracing;
using System.Net;
using System.Text;
using XLeech.Core;
using XLeech.Core.Extensions;
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
        private ParallelCrawlerEngine _parallelCrawlerEngine;
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
            if (Main.AppWindow.ParallelCrawlerEngine != null)
            {
                _parallelCrawlerEngine = Main.AppWindow.ParallelCrawlerEngine;

                categoryCrawled = _parallelCrawlerEngine.Impls.ImplementationBag.CategoryCrawled;
                LogLabel(this.CategoryCrawledLb, categoryCrawled.ToString());

                postSuccess = _parallelCrawlerEngine.Impls.ImplementationBag.PostSuccess;
                LogLabel(this.PostSuccessLb, postSuccess.ToString());

                postFailed = _parallelCrawlerEngine.Impls.ImplementationBag.PostFailed;
                LogLabel(this.PostFailedLb, postFailed.ToString());

                _parallelCrawlerEngine.CrawlerInstanceCreated += CrawlerInstanceCreated;

                _parallelCrawlerEngine.SiteCrawlStarting += (sender, args) =>
                {

                };

                _parallelCrawlerEngine.SiteCrawlCompleted += async (sender, siteCrawleArgs) =>
                {
                    var siteConfig = siteCrawleArgs.CrawledSite.SiteToCrawl.SiteBag.SiteConfig as SiteConfig;
                    var categoryNextPageURL = siteCrawleArgs.CrawledSite.SiteToCrawl.SiteBag.CategoryNextPageURL as string;
                    siteConfig.CategoryNextPageURL = categoryNextPageURL;
                    siteConfig.IsDone = string.IsNullOrEmpty(categoryNextPageURL);
                    await _siteConfigRepository.UpdateAsync(siteConfig);
                    if (!siteConfig.IsDone)
                    {
                        CrawleSite(siteConfig);
                    }
                };

                _parallelCrawlerEngine.AllCrawlsCompleted += async (sender, eventArgs) =>
                {
                    Interlocked.Increment(ref categoryCrawled);
                    LogLabel(this.CategoryCrawledLb, categoryCrawled.ToString());
                };
            }
        }

        private async void CrawlerInstanceCreated(object sender, CrawlerInstanceCreatedArgs eventArgs)
        {
            eventArgs.Crawler.CrawlBag.SiteConfig = eventArgs.SiteToCrawl.SiteBag.SiteConfig;
            eventArgs.Crawler.PageCrawlCompleted += PageCrawlCompleted;

            var postUrls = eventArgs.SiteToCrawl.SiteBag.PostUrls as List<string>;
            eventArgs.Crawler.CrawlContext.Scheduler.Add(postUrls.Select(u => new PageToCrawl(new Uri(u))));
        }

        private async void PageCrawlCompleted(object abotSender, PageCrawlCompletedArgs abotEventArgs)
        {
            var siteConfig = abotEventArgs.CrawlContext.CrawlBag.SiteConfig as SiteConfig;

            try
            {
                if (string.IsNullOrEmpty(abotEventArgs.CrawledPage.Content.Text) || siteConfig == null) throw new Exception("Content empty");

                var crawlerResult = await _crawlerService.PageCrawlCompleted(abotSender, abotEventArgs, siteConfig);

                Interlocked.Increment(ref postSuccess);
                _parallelCrawlerEngine.Impls.ImplementationBag.PostSuccess = postSuccess;

                LogLabel(this.PostSuccessLb, postSuccess.ToString());
                LogPost(string.Format("{0} Completed, {1}", abotEventArgs.CrawledPage.Uri, crawlerResult.IsSavePost ? "Save" : "Skiped"));
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref postFailed);
                _parallelCrawlerEngine.Impls.ImplementationBag.PostFailed = postFailed;

                LogLabel(this.PostFailedLb, postFailed.ToString());
                LogPost(string.Format("{0} Exception {1}", siteConfig?.Url, ex.Message));
            }
        }

        private static CrawlConfigurationX GetSafeConfig()
        {
            return new CrawlConfigurationX
            {
                MaxPagesToCrawl = 1,
                MinCrawlDelayPerDomainMilliSeconds = 10000,
                MaxConcurrentSiteCrawls = 3,
                IsSendingCookiesEnabled = true
                //HttpRequestTimeoutInSeconds= 60,
                //MaxConcurrentThreads = 5,
            };
        }

        private async Task CrawleEngineAsync()
        {
            var sites = _dbContext.Sites
                       .Where(x => x.ActiveForScheduling && !x.IsDone)
                       .Include(x => x.Category)
                       .Include(y => y.Post)
                       .AsNoTracking()
                       .ToList();
            this.AllSiteLb.Text = sites?.Count.ToString();
            if (sites != null && sites.Any())
            {
                foreach (var site in sites)
                {
                    CrawleSite(site);
                }
            }
            else
            {
                MessageBox.Show("No sites to crawle");
            }
        }


        private async Task<ParallelCrawlerEngine> CrawleSite(SiteConfig siteConfig)
        {
            var siteToCrawls = new List<SiteToCrawl>();
            var config = GetSafeConfig();

            try
            {
                // get urls from post list
                if (siteConfig.IsPageUrl == true)
                {
                    string[] postUrls = siteConfig.Category.Urls?.ToListString();
                    if (postUrls != null && postUrls.Any())
                    {
                        siteToCrawls.AddRange(
                            postUrls.Select(x => new SiteToCrawl
                            {
                                Uri = new Uri(x),
                                SiteBag = siteConfig
                            }).ToList()
                        );
                    }
                }

                // get urls from url category page
                if (siteConfig.IsPageUrl == false)
                {
                    var urlCategoryPageCrawle = _crawlerService.GetURLCategoryPageCrawle(siteConfig);
                    var categoryPageInfo = await _crawlerService.GetInfoCategoryPage(siteConfig, config);
                    if (categoryPageInfo != null && categoryPageInfo.PostUrls.Any())
                    {
                        var siteToCrawle = new SiteToCrawl
                        {
                            Uri = new Uri(urlCategoryPageCrawle)
                        };
                        siteToCrawle.SiteBag.SiteConfig = siteConfig;
                        siteToCrawle.SiteBag.PostUrls = categoryPageInfo.PostUrls;
                        siteToCrawle.SiteBag.CategoryNextPageURL = categoryPageInfo.CategoryNextPageURL;
                        siteToCrawls.Add(siteToCrawle);
                    }
                }

                _parallelCrawlerEngine.Impls.SiteToCrawlProvider.AddSitesToCrawl(siteToCrawls);
            }
            catch (Exception ex)
            {
                LogSite(string.Format("{0} Exception {1}", siteConfig.Url, ex.Message));
            }

            return new ParallelCrawlerEngine(config);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            CrawleEngineAsync();
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

            CrawleEngineAsync();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            _parallelCrawlerEngine.Stop();
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            _parallelCrawlerEngine.Pause();
        }

        private void ResumeBtn_Click(object sender, EventArgs e)
        {
            _parallelCrawlerEngine.Resume();
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

        #endregion Log
    }
}