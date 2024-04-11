using Abot2.Crawler;
using AbotX2.Crawler;
using AbotX2.Poco;
using AngleSharp.Html.Dom;
using WordPressPCL.Models;
using XLeech.Core.Model;
using XLeech.Data.Entity;

namespace XLeech.Core.Service
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IChatGPTService _chatGPTService;
        public CrawlerService(IChatGPTService chatGPTService) {
            _chatGPTService = chatGPTService;
        }

        public async Task<CrawlerResult> PageCrawlCompleted(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig)
        {
            var crawlerResult = new CrawlerResult();

            if (siteConfig.IsPageUrl)
            {
                crawlerResult = await PageCrawlCompletedUrl(abotSender, abotEventArgs, siteConfig);
            }
            else
            {
                crawlerResult = await PageCrawlCompletedCategoryPage(abotSender, abotEventArgs, siteConfig);
            }

            return await Task.FromResult(crawlerResult);
        }

        public async Task<CrawlerResult> PageCrawlCompletedCategoryPage(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig)
        {
            var crawledPage = abotEventArgs.CrawledPage;
            var wordpressProcessor = new WordpressProcessor(siteConfig);
            var crawlerResult = new CrawlerResult()
            {
                IsSaveCategory = false,
                IsError = false,
                IsSavePost = false
            };

            var categoryModel = wordpressProcessor.GetCategory(crawledPage.AngleSharpHtmlDocument, siteConfig);
            var existCategory = await wordpressProcessor.IsExistCategory(categoryModel, siteConfig);
            var category = new Category()
            {
                Id = existCategory?.Id ?? 0
            };
            if (existCategory == null)
            {
                category = await wordpressProcessor.SaveCategory(categoryModel);
                crawlerResult.IsSaveCategory = true;
            }

            var postModel = wordpressProcessor.GetPost(crawledPage.AngleSharpHtmlDocument, siteConfig);
            if (postModel == null)
            {
               throw new Exception(string.Format("Not found post"));
            }
            var existPost = await wordpressProcessor.IsExistPost(postModel, siteConfig);
            if (existPost == null)
            {
                await wordpressProcessor.SavePost(postModel, new List<int>() { existCategory != null ? existCategory.Id : category.Id });
                crawlerResult.IsSavePost = true;
            }

            return await Task.FromResult(crawlerResult);
        }

        public async Task<CrawlerResult> PageCrawlCompletedUrl(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig)
        {
            var crawledPage = abotEventArgs.CrawledPage;
            var wordpressProcessor = new WordpressProcessor(siteConfig);
            var crawlerResult = new CrawlerResult()
            {
                IsSaveCategory = false,
                IsSavePost = false,
                IsError = false,
            };

            var categoryModel = wordpressProcessor.GetCategory(crawledPage.AngleSharpHtmlDocument, siteConfig);
            var existCategory = await wordpressProcessor.IsExistCategory(categoryModel, siteConfig);
            var category = new Category()
            {
                Id = existCategory?.Id ?? 0
            };
            if (existCategory == null)
            {
                category = await wordpressProcessor.SaveCategory(categoryModel);
                crawlerResult.IsSaveCategory = true;
            }

            var postModel = wordpressProcessor.GetPost(crawledPage.AngleSharpHtmlDocument, siteConfig);
            if (postModel == null)
            {
                throw new Exception(string.Format("Not found post"));
            }
            var existPost = await wordpressProcessor.IsExistPost(postModel, siteConfig);
            if (existPost == null)
            {
                await wordpressProcessor.SavePost(postModel, new List<int>() { existCategory != null ? existCategory.Id : category.Id });
                crawlerResult.IsSavePost = true;
            }

            return await Task.FromResult(crawlerResult);
        }

        public Task<List<string>> GetPostUrls(IHtmlDocument document, SiteConfig siteConfig)
        {
            var postUrls = document.QuerySelectorAll(siteConfig.Category.CategoryPostURLSelector)
                .Select(ele => ele.GetAttribute("href") ?? ele.GetAttribute("src"))
                .ToList();
            return Task.FromResult(postUrls.Where(x => !string.IsNullOrEmpty(x)).ToList());
        }

        public async Task<CategoryPageInfo> GetInfoCategoryPage(SiteConfig siteConfig, CrawlConfigurationX crawlConfigurationX)
        {
            var categoryPageInfo = new CategoryPageInfo();
            crawlConfigurationX = MergeCrawlConfiguration(crawlConfigurationX, siteConfig);
            using (var crawler = new CrawlerX(crawlConfigurationX))
            {
                string urlCategoryPageCrawle = GetURLCategoryPageCrawle(siteConfig);
                crawler.PageCrawlCompleted += async (sender, args) =>
                {
                    var wordpressProcessor = new WordpressProcessor(siteConfig);

                    var crawlerPage = args.CrawledPage;
                    categoryPageInfo.PostUrls = await GetPostUrls(crawlerPage.AngleSharpHtmlDocument, siteConfig);
                    categoryPageInfo.CategoryNextPageURL = GetNexCategoryPostURL(crawlerPage.AngleSharpHtmlDocument, siteConfig);
                };

                await crawler.CrawlAsync(new Uri(urlCategoryPageCrawle));
            }

            return categoryPageInfo;
        }

        public string GetURLCategoryPageCrawle(SiteConfig siteConfig)
        {
            return siteConfig.CategoryNextPageURL ?? siteConfig.Category.CategoryPostURL;
        }

        public string GetNexCategoryPostURL(IHtmlDocument document, SiteConfig siteConfig)
        {
            var ele = document.QuerySelector(siteConfig.Category.CategoryNextPageURLSelector)?.NextElementSibling;
            if (ele == null) return null;

            return ele?.GetAttribute("href") ?? ele?.GetAttribute("src");
        }

        public CrawlConfigurationX MergeCrawlConfiguration(CrawlConfigurationX config, SiteConfig siteConfig)
        {
            if (!string.IsNullOrEmpty(siteConfig.HTTPUserAgent))
            {
                config.UserAgentString = siteConfig.HTTPUserAgent;
            }

            return config;
        }
    }
}