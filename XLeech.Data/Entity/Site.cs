﻿
namespace XLeech.Data.Entity
{
    public class Site: BaseEntity
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public bool IsUrl {get;set;}
        public bool ActiveForScheduling { get; set; }
        public bool CheckDuplicatePostViaUrl { get; set; }
        public bool CheckDuplicatePostViaTitle { get; set; }
        public bool CheckDuplicatePostViaContent { get; set; }
        public int? MaximumPagesCrawlPerCategory { get; set; }
        public int? MaximumPagesCrawlPerPost { get; set; }
        public string? HTTPUserAgent { get; set; }
        public string? Cookie { get; set; }
        public int ConnectionTimeout { get; set; }
        public bool UseProxy { get; set; }
        /// <summary>
        /// New line seprated proxies
        /// </summary>
        public string? Proxies { get; set; }
        public int ProxyRetryLimit { get; set; }
        public bool RandomizeProxies { get; set; }
        public int TimeInterval { get; set; }
        public DateTime? LatestRun { get; set; }
        public string? LastUrlCollection { get; set; }
        public string? LastPostCrawl { get; set; }
        public string? Notes { get; set; }
        public Category Category { get; set; }
        public Post Post { get; set; }
    }
}