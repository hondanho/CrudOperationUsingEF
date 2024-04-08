using Abot2.Crawler;
using AbotX2.Crawler;
using AbotX2.Poco;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using WordPressPCL.Models;
using XLeech.Core.Extensions;
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
            var wordpressProcessor = new WordpressProcessor(siteConfig);

            if (siteConfig.IsPageUrl)
            {
                crawlerResult = await PageCrawlCompletedUrl(abotSender, abotEventArgs, siteConfig, wordpressProcessor);
            }
            else
            {
                crawlerResult = await PageCrawlCompletedCategoryPage(abotSender, abotEventArgs, siteConfig, wordpressProcessor);
            }

            return await Task.FromResult(crawlerResult);
        }

        public async Task<CrawlerResult> PageCrawlCompletedCategoryPage(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig, IProccessor proccessor)
        {
            var crawledPage = abotEventArgs.CrawledPage;
            var crawlerResult = new CrawlerResult()
            {
                IsSaveCategory = false,
                IsError = false,
                IsSavePost = false
            };

            var categoryModel = GetCategory(crawledPage.AngleSharpHtmlDocument, siteConfig);
            var existCategory = await proccessor.IsExistCategory(categoryModel, siteConfig);
            var category = new Category()
            {
                Id = existCategory?.Id ?? 0
            };
            if (existCategory == null)
            {
                category = await proccessor.SaveCategory(categoryModel);
                crawlerResult.IsSaveCategory = true;
            }

            var postModel = GetPost(crawledPage.AngleSharpHtmlDocument, siteConfig);
            if (postModel == null)
            {
               throw new Exception(string.Format("Not found post"));
            }
            var existPost = await proccessor.IsExistPost(postModel, siteConfig);
            if (existPost == null)
            {
                await proccessor.SavePost(postModel, new List<int>() { existCategory != null ? existCategory.Id : category.Id });
                crawlerResult.IsSavePost = true;
            }

            return await Task.FromResult(crawlerResult);
        }

        public async Task<CrawlerResult> PageCrawlCompletedUrl(object? abotSender, PageCrawlCompletedArgs abotEventArgs, SiteConfig siteConfig, IProccessor processor)
        {
            var crawledPage = abotEventArgs.CrawledPage;
            var crawlerResult = new CrawlerResult()
            {
                IsSaveCategory = false,
                IsSavePost = false,
                IsError = false,
            };

            var categoryModel = GetCategory(crawledPage.AngleSharpHtmlDocument, siteConfig);
            var existCategory = await processor.IsExistCategory(categoryModel, siteConfig);
            var category = new Category()
            {
                Id = existCategory?.Id ?? 0
            };
            if (existCategory == null)
            {
                category = await processor.SaveCategory(categoryModel);
                crawlerResult.IsSaveCategory = true;
            }

            var postModel = GetPost(crawledPage.AngleSharpHtmlDocument, siteConfig);
            if (postModel == null)
            {
                throw new Exception(string.Format("Not found post"));
            }
            var existPost = await processor.IsExistPost(postModel, siteConfig);
            if (existPost == null)
            {
                await processor.SavePost(postModel, new List<int>() { existCategory != null ? existCategory.Id : category.Id });
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

        public async Task<CategoryPageInfo> GetCategoryNextPageInfo(SiteConfig siteConfig, CrawlConfigurationX crawlConfigurationX)
        {
            var categoryPageInfo = new CategoryPageInfo();
            crawlConfigurationX = MergeCrawlConfiguration(crawlConfigurationX, siteConfig);
            using (var crawler = new CrawlerX(crawlConfigurationX))
            {
                crawler.PageCrawlCompleted += async (sender, args) =>
                {
                    var wordpressProcessor = new WordpressProcessor(siteConfig);

                    var crawlerPage = args.CrawledPage;
                    categoryPageInfo.PostUrls = await GetPostUrls(crawlerPage.AngleSharpHtmlDocument, siteConfig);
                    categoryPageInfo.CategoryNextPageURL = GetNexCategoryPostURL(crawlerPage.AngleSharpHtmlDocument, siteConfig);
                };

                await crawler.CrawlAsync(new Uri(GetCategoryPageURLCrawle(siteConfig)));
            }

            return categoryPageInfo;
        }

        public string GetCategoryPageURLCrawle(SiteConfig siteConfig)
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

        public CategoryModel GetCategory(IHtmlDocument document, SiteConfig siteConfig)
        {
            var categoryModel = new CategoryModel()
            {
                Name = siteConfig.Category.CategoryMap,
                Slug = siteConfig.Category.CategoryMap.GenerateSlug()
            };

            // description
            if (!string.IsNullOrEmpty(siteConfig.Category.Description))
            {
                categoryModel.Description = document.QuerySelector(siteConfig.Category.Description)?.Text();
            }

            // feature image
            if (siteConfig.Category.SaveFeaturedImages && !string.IsNullOrEmpty(siteConfig.Category.FeaturedImageSelector))
            {
                var imageEle = document.QuerySelector(siteConfig.Category.FeaturedImageSelector);
                var featureImage = imageEle.GetAttribute("href") ?? imageEle.GetAttribute("src");
                categoryModel.FeatureImage = featureImage;
            }

            return categoryModel;
        }

        public PostModel GetPost(IHtmlDocument document, SiteConfig siteConfig)
        {
            var slug = document.QuerySelector(siteConfig.Post.PostSlugSelector)?.GetAttribute("href")?.GetAbsolutePath();
            var title = document.QuerySelector(siteConfig.Post.PostTitleSelector)?.Text();

            // remove unnecessary element selectors
            var eleToRemoveSelectors = siteConfig.Post.UnnecessaryElements?.ToListString();
            if (eleToRemoveSelectors.Any())
            {
                foreach (var eleSelector in eleToRemoveSelectors)
                {
                    var nodeToRemoves = document.QuerySelector(siteConfig.Post.PostContentSelector)?.QuerySelectorAll(eleSelector);
                    if (nodeToRemoves != null && nodeToRemoves.Any())
                    {
                        foreach (var node in nodeToRemoves)
                        {
                            node.Remove();
                        }
                    }
                }
            }
            var content = document.QuerySelector(siteConfig.Post.PostContentSelector)?.InnerHtml;

            if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) return null;

            var postModel = new PostModel()
            {
                Title = title,
                Author = siteConfig.Post.PostAuthor,
                Format = siteConfig.Post.PostFormat,
                Status = siteConfig.Post.PostStatus,
                Type = siteConfig.Post.PostType,
                Slug = slug,
                Content = content
            };

            // feature image
            if (siteConfig.Post.SaveFeaturedImages)
            {
                var imageEle = document.QuerySelector(siteConfig.Post.FeaturedImageSelector);
                var featureImage = imageEle.GetAttribute("href") ?? imageEle.GetAttribute("src");
                postModel.FeatureImage = featureImage;
            }

            return postModel;
        }
    }
}