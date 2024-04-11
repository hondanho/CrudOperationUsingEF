using AbotX2.Parallel;
using AbotX2.Poco;

namespace XLeech.Core
{
    public class ParallelCrawlerEngineGlobal
    {
        private static ParallelCrawlerEngine _instance;

        private ParallelCrawlerEngineGlobal() {
        }

        public static ParallelCrawlerEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    var config = GetSafeConfig();
                    var siteToCrawlProvider = new AlwaysOnSiteToCrawlProvider();
                    _instance = new ParallelCrawlerEngine(
                        config,
                        new ParallelImplementationOverride(config,
                                new ParallelImplementationContainer()
                                {
                                    SiteToCrawlProvider = siteToCrawlProvider,
                                    WebCrawlerFactory = new WebCrawlerFactory(config)
                                }
                            )
                        );

                }

                return _instance;
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
    }
}
