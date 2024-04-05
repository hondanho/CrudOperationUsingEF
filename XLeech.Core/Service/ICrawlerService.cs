
using Abot2.Crawler;
using AbotX2.Poco;
using AngleSharp.Html.Dom;
using XLeech.Core.Model;
using XLeech.Data.Entity;

namespace XLeech.Core.Service
{
    public interface ICrawlerService
    {
        string GetCategoryPageURLCrawle(SiteConfig siteConfig);
        Task<CategoryPageInfo> GetCategoryNextPageInfo(SiteConfig siteConfig, CrawlConfigurationX crawlConfigurationX);
        Task<CrawlerResult> PageCrawlCompleted(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig);
        Task<CrawlerResult> PageCrawlCompletedCategoryPage(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig);
        Task<CrawlerResult> PageCrawlCompletedUrl(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig);
        Task<List<string>> GetPostUrls(IHtmlDocument document, SiteConfig siteConfig);
        string GetNexCategoryPostURL(IHtmlDocument document, SiteConfig siteConfig);
    }
}
