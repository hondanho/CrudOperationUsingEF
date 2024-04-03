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
        public async Task PageCrawlCompletedCategoryPage(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig)
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

        public async Task PageCrawlCompleted(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig)
        {
            if (siteConfig.IsPageUrl)
            {
                await PageCrawlCompletedUrl(abotSender, abotEventArgs, siteConfig);
            }
            else
            {
                await PageCrawlCompletedCategoryPage(abotSender, abotEventArgs, siteConfig);
            }
        }

        public async Task PageCrawlCompletedUrl(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig)
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

        public Task<List<string>> GetPostUrls(IHtmlDocument document, SiteConfig siteConfig)
        {
            var postUrls = document.QuerySelectorAll(siteConfig.Category.CategoryPostURLSelector)
                .Select(ele => ele.GetAttribute("href") ?? ele.GetAttribute("src"))
                .ToList();
            return Task.FromResult(postUrls.Where(x => !string.IsNullOrEmpty(x)).ToList());
        }

        public async Task<CategoryPageInfo> GetCategoryNextPageInfo(SiteConfig siteConfig, CrawlConfigurationX crawlConfigurationX)
        {
            var categoryPageInfo = new CategoryPageInfo();
            using (var crawler = new CrawlerX(crawlConfigurationX))
            {
                crawler.PageCrawlCompleted += async (sender, args) =>
                {
                    var wordpressProcessor = new WordpressProcessor(siteConfig);

                    var crawlerPage = args.CrawledPage;
                    categoryPageInfo.PostUrls = await GetPostUrls(crawlerPage.AngleSharpHtmlDocument, siteConfig);
                    categoryPageInfo.CategoryNextPageURL = await GetNexCategoryPostURL(crawlerPage.AngleSharpHtmlDocument, siteConfig);
                };

                await crawler.CrawlAsync(new Uri(GetCategoryPageURLCrawle(siteConfig)));
            }

            return categoryPageInfo;
        }

        public string GetCategoryPageURLCrawle(SiteConfig siteConfig)
        {
            return siteConfig.CategoryNextPageURL ?? siteConfig.Category.CategoryPostURL;
        }

        public Task<string?> GetNexCategoryPostURL(IHtmlDocument document, SiteConfig siteConfig)
        {
            var ele = document.QuerySelector(siteConfig.Category.CategoryNextPageURLSelector).NextElementSibling;
            return Task.FromResult(ele?.GetAttribute("href") ?? ele?.GetAttribute("src"));
        }
    }
}