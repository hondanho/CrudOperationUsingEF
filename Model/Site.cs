using System.ComponentModel.DataAnnotations;

namespace XAutoLeech.Model
{
    public class Site
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool ActiveForScheduling { get; set; }
        public bool CheckDuplicatePostViaUrl { get; set; }
        public bool CheckDuplicatePostViaTitle { get; set; }
        public bool CheckDuplicatePostViaContent { get; set; }
        public Nullable<int> MaximumPagesCrawlPerCategory { get; set; }
        public Nullable<int> MaximumPagesCrawlPerPost { get; set; }
        public bool AllowComment { get; set; }
        public string PostStatus { get; set; }
        public string PostType { get; set; }
        public string PostAuthor { get; set; }
        public string HTTPUserAgent { get; set; }
        public int ConnectionTimeout { get; set; }
        public bool UseProxy { get; set; }
        /// <summary>
        /// New line seprated proxies
        /// </summary>
        public string Proxies { get; set; }
        public int ProxyRetryLimit { get; set; }
        public bool RandomizeProxies { get; set; }
        public int TimeInterval { get; set; }
        public DateTime LatestRun { get; set; }
        public string LastUrlCollection { get; set; }
        public string LastPostCrawl { get; set; }
        public string Notes { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime CreateTime { get; set; }
    }
}