
using Abot2.Crawler;
using AbotX2.Poco;
using AngleSharp.Html.Dom;
using XLeech.Core.Model;
using XLeech.Data.Entity;

namespace XLeech.Core.Service
{
    public interface ICrawlerService
    {
        string GetURLCategoryPageCrawle(SiteConfig siteConfig);
        Task<CategoryPageInfo> GetInfoCategoryPage(SiteConfig siteConfig, CrawlConfigurationX crawlConfigurationX);
        PostModel GetPost(IHtmlDocument document, SiteConfig siteConfig);
        CategoryModel GetCategory(IHtmlDocument document, SiteConfig siteConfig);
        Task<CrawlerResult> PageCrawlCompleted(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig);
        Task<CrawlerResult> PageCrawlCompletedCategoryPage(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig, IProccessor processor);
        Task<CrawlerResult> PageCrawlCompletedUrl(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig, IProccessor processor);
        Task<List<string>> GetPostUrls(IHtmlDocument document, SiteConfig siteConfig);
        string GetNexCategoryPostURL(IHtmlDocument document, SiteConfig siteConfig);
    }
}
