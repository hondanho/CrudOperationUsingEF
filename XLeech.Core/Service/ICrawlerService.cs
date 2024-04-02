
using Abot2.Crawler;
using AbotX2.Poco;
using AngleSharp.Html.Dom;
using XLeech.Core.Model;
using XLeech.Data.Entity;

namespace XLeech.Core.Service
{
    interface ICrawlerService
    {
        Task<CategoryPageInfo> GetCategoryNextPageInfo(SiteConfig siteConfig, CrawlConfigurationX crawlConfigurationX);
        Task PageCrawlCompletedCategoryPage(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig);
        Task PageCrawlCompletedUrl(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig);
        Task<List<string>> GetPostUrls(IHtmlDocument document, SiteConfig siteConfig);
        Task<string> GetNexCategoryPostURL(IHtmlDocument document, SiteConfig siteConfig);
    }
}
