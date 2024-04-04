using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using WordPressPCL;
using WordPressPCL.Client;
using WordPressPCL.Models;
using WordPressPCL.Utility;
using XLeech.Core.Extensions;
using XLeech.Core.Model;
using XLeech.Data.Entity;

namespace XLeech.Core
{
    public class WordpressProcessor : IProccessor
    {
        private WordPressClient _wordPressClient { get; set; }
        private const string UriApi = "https://bepxinhhanoi.com/wp-json/";
        private const string UserName = "admin";
        private const string Password = "b3sR lCNw aPla aMZ9 XvKt qi0j";

        public WordpressProcessor(SiteConfig siteConfig)
        {
            _wordPressClient = new WordPressClient(!string.IsNullOrEmpty(siteConfig.WordpressApiUrl) ? siteConfig.WordpressApiUrl : UriApi);
            _wordPressClient.Auth.UseBasicAuth(
                    !string.IsNullOrEmpty(siteConfig.WordpressUserName) ? siteConfig.WordpressUserName : UserName,
                    !string.IsNullOrEmpty(siteConfig.WordpressApplicationPW) ? siteConfig.WordpressApplicationPW : Password
                );
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

        public async Task<CategoryModel> IsExistCategory(CategoryModel categoryModel, SiteConfig siteConfig)
        {
            var query = new CategoriesQueryBuilder();

            if (siteConfig.CheckDuplicatePostViaUrl)
            {
                query.Slugs = new List<string> { categoryModel.Slug };
            }
            
            var categorie = (await _wordPressClient.Categories.QueryAsync(query))?.FirstOrDefault();
            categoryModel.Id = categorie?.Id ?? 0;
            return categorie != null ? await Task.FromResult(categoryModel) : null;
        }

        public async Task<PostModel> IsExistPost(PostModel postModel, SiteConfig siteConfig)
        {
            var query = new PostsQueryBuilder();

            if (siteConfig.CheckDuplicatePostViaUrl)
            {
                query.Slugs = new List<string> { postModel.Slug };
            }
            if (siteConfig.CheckDuplicatePostViaTitle)
            {
                query.Search = postModel.Title;
            }

            var post = (await _wordPressClient.Posts.QueryAsync(query))?.FirstOrDefault();
            postModel.Id = post?.Id ?? 0;
            return post != null ? await Task.FromResult(postModel) : null;
        }

        public async Task<bool> SavePost(PostModel postModel, List<int>? cateogoryIds)
        {
            try
            {
                int? featureId = 0;
                if (!string.IsNullOrEmpty(postModel.FeatureImage))
                {
                    try
                    {
                        var fileFeatureName = Path.GetFileName(postModel.FeatureImage);
                        var feature = await _wordPressClient.Media.CreateAsync(postModel.FeatureImage, fileFeatureName);
                        featureId = feature.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex?.Message);
                    }
                }

                var status = Status.Publish;
                if (postModel.Status == "pending") status = Status.Pending;
                if (postModel.Status == "draft") status = Status.Draft;

                var postWp = await _wordPressClient.Posts.CreateAsync(new Post
                {
                    Title = new Title(postModel.Title),
                    Slug = postModel.Slug,
                    Categories = cateogoryIds,
                    Content = new Content(postModel.Content),
                    Status = status,
                    FeaturedMedia = featureId > 0 ? featureId : null,
                    Meta = new
                    {
                        postModel.Status,
                        postModel.Slug
                    }
                });
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<Category> SaveCategory(CategoryModel categoryModel)
        {
            var categoryWp = await _wordPressClient.Categories.CreateAsync(new Category
            {
                Slug = categoryModel.Slug,
                Description = categoryModel.Description,
                Name = categoryModel.Name,
                Meta = categoryModel.Meta,
                Taxonomy = "category"
            });

            return await Task.FromResult(categoryWp);
        }

        //public async Task SyncDataBySite(SiteConfigDb request)
        //{
        //    // init

        //    var wpClient = new WordPressClient(request.BasicSetting.WordpressUriApi ?? wordpressUriApi);
        //    wpClient.Auth.UseBasicAuth(
        //        request.BasicSetting.WordpressUserName ?? wordpressUserName,
        //        request.BasicSetting.WordpressPassword ?? wordpressPassword
        //    );

        //    if (request.CategorySetting.CategoryModels != null && request.CategorySetting.CategoryModels.Any())
        //    {
        //        var categoryWps = await wpClient.Categories.GetAllAsync();
        //        var tacGiaWps = await wpClient.CustomRequest.GetAsync<List<TacGiaWp>>($"/wp-json/wp/v2/all-terms?term=tac-gia");
        //        var postDbs = _postDbRepository.FilterBy(pdb =>
        //                            request.CategorySetting.CategoryModels.Any(csm => csm.Slug == pdb.CategorySlug)
        //                        ).ToList();
        //        if (!postDbs.Any()) return;

        //        var listCategoryNews = new List<Category>();
        //        var listTacGiaNews = new List<TacGiaWp>();

        //        foreach (var postDb in postDbs.Where(pdb => pdb.Metadatas.Any()))
        //        {
        //            // sync genre
        //            var metadataGenres = postDb.Metadatas.FirstOrDefault(metadata =>
        //                metadata.Key == MetaFieldPost.Genre &&
        //                metadata.Value != null &&
        //                metadata.Value.Any()
        //            );
        //            foreach (var categoryTxt in metadataGenres.Value)
        //            {
        //                var slug = categoryTxt.Trim().Replace(" ", "-").ToLower();
        //                if (!categoryWps.Any(cwp => cwp.Slug == slug) && !listCategoryNews.Any(category => category.Slug == slug))
        //                {
        //                    listCategoryNews.Add(new Category()
        //                    {
        //                        Slug = slug,
        //                        Name = categoryTxt.Trim(),
        //                        Taxonomy = "category"
        //                    });
        //                }
        //            }

        //            // sync tac-gia
        //            var metadataTacGia = postDb.Metadatas.FirstOrDefault(metadata =>
        //                metadata.Key == MetaFieldPost.TacGia &&
        //                metadata.Value != null &&
        //                metadata.Value.Any()
        //            );
        //            foreach (var tacgiaTxt in metadataTacGia.Value)
        //            {
        //                var slug = tacgiaTxt.Trim().Replace(" ", "-").ToLower();
        //                if (!tacGiaWps.Any(twp => twp.Name == tacgiaTxt || twp.Slug == slug) &&
        //                    !listTacGiaNews.Any(tacgia => tacgia.Name == tacgiaTxt || tacgia.Slug == slug))
        //                {
        //                    listTacGiaNews.Add(new TacGiaWp
        //                    {
        //                        Slug = slug,
        //                        Name = tacgiaTxt.Trim(),
        //                        Taxonomy = "tac-gia"
        //                    });
        //                }
        //            }
        //        }

        //        if (listCategoryNews.Any())
        //        {
        //            foreach (var category in listCategoryNews)
        //            {
        //                try
        //                {
        //                    var categoryWp = await wpClient.Categories.CreateAsync(category);
        //                    Console.WriteLine(string.Format("CATEGORY SYNCED: Id: {0}, Slug: {0}, Title: {1}", categoryWp.Id, categoryWp.Slug, categoryWp.Name));
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex?.Message);
        //                }
        //            }
        //        }

        //        if (listTacGiaNews.Any())
        //        {
        //            foreach (var tacgiaWp in listTacGiaNews)
        //            {
        //                try
        //                {
        //                    var tacgia = await wpClient.CustomRequest.CreateAsync<TacGiaWp, TacGiaWp>($"/wp-json/wp/v2/tac-gia", tacgiaWp);
        //                    Console.WriteLine(string.Format("TAC GIA SYNCED: Id: {0}, Slug: {0}, Title: {1}", tacgia.Id, tacgia.Slug, tacgia.Name));
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex?.Message);
        //                }
        //            }
        //        }

        //        await SyncPost(wpClient, request);
        //    }
        //}

        //private async Task SyncPost(WordPressClient wpClient, SiteConfigDb request)
        //{
        //    var tacGiaWps = await wpClient.CustomRequest.GetAsync<List<TacGiaWp>>($"/wp-json/wp/v2/all-terms?term=tac-gia");
        //    var categoryWps = await wpClient.Categories.GetAllAsync();
        //    var categoryDbs = _categoryDbRepository.AsQueryable().ToList();

        //    foreach (var categoryDb in categoryDbs)
        //    {
        //        // sync post
        //        var postLogs = _postLogRepository.FilterBy(pl => pl.CategorySlug == categoryDb.Slug).ToList();
        //        var postDbs = _postDbRepository.FilterBy(pdb => pdb.CategorySlug == categoryDb.Slug).ToList();
        //        if (!postDbs.Any()) return;
        //        foreach (var postDb in postDbs)
        //        {
        //            // sync post
        //            var postLog = postLogs.FirstOrDefault(pl => pl.Slug == postDb.Slug && pl.PostWpId > 0 && pl.CategorySlug == categoryDb.Slug);
        //            if (postLog != null && !string.IsNullOrEmpty(postDb.Slug))
        //            {
        //                var chapsLogCount = _chapLogRepository.FilterBy(cwp => cwp.PostSlug == postDb.Slug).Count();
        //                var chapDbCount = _chapDbRepository.FilterBy(cdb => cdb.PostSlug == postDb.Slug).Count();
        //                if (chapDbCount == chapsLogCount) continue;
        //            }
        //            else
        //            {
        //                var categoryIds = new List<int>();
        //                var tacgiaIds = new List<int>();

        //                var metadataGenres = postDb.Metadatas.FirstOrDefault(metadata =>
        //                                    metadata.Key == MetaFieldPost.Genre &&
        //                                    metadata.Value != null &&
        //                                    metadata.Value.Any()
        //                                );
        //                foreach (var categoryTxt in metadataGenres.Value.Where(x => !string.IsNullOrEmpty(x)))
        //                {
        //                    var slug = categoryTxt.Trim().Replace(" ", "-").ToLower();
        //                    var categoryWp = categoryWps.FirstOrDefault(cwp => cwp.Slug == slug);
        //                    if (categoryWp != null)
        //                    {
        //                        categoryIds.Add(categoryWp.Id);
        //                    }
        //                }

        //                var metadataTacGia = postDb.Metadatas.FirstOrDefault(metadata =>
        //                    metadata.Key == MetaFieldPost.TacGia &&
        //                    metadata.Value != null &&
        //                    metadata.Value.Any()
        //                );
        //                foreach (var tacgiaTxt in metadataTacGia.Value)
        //                {
        //                    var slug = tacgiaTxt.Trim().Replace(" ", "-").ToLower();
        //                    var tacgiaWp = tacGiaWps.FirstOrDefault(cwp => cwp.Slug == slug);
        //                    if (tacgiaWp != null)
        //                    {
        //                        tacgiaIds.Add(tacgiaWp.TermID);
        //                    }
        //                }

        //                var metadataSource = postDb.Metadatas.FirstOrDefault(metadata =>
        //                    metadata.Key == MetaFieldPost.Source &&
        //                    metadata.Value != null &&
        //                    metadata.Value.Any()
        //                );
        //                var metadataStatus = postDb.Metadatas.FirstOrDefault(metadata =>
        //                    metadata.Key == MetaFieldPost.Status &&
        //                    metadata.Value != null &&
        //                    metadata.Value.Any()
        //                );
        //                var metadataAlternativeName = postDb.Metadatas.FirstOrDefault(metadata =>
        //                    metadata.Key == MetaFieldPost.AlternativeName &&
        //                    metadata.Value != null &&
        //                    metadata.Value.Any()
        //                );

        //                int? featureId = 0;
        //                if (!string.IsNullOrEmpty(postDb.Avatar))
        //                {
        //                    try
        //                    {
        //                        var pathAvatar = string.Empty;
        //                        if (!Helper.IsValidURL(postDb.Avatar)) pathAvatar = request.BasicSetting.Domain + postDb.Avatar;

        //                        Uri uriAvatar = new Uri(pathAvatar);
        //                        string filePath = uriAvatar.AbsolutePath;
        //                        string fileName = Path.GetFileName(filePath);
        //                        var pathSave = string.Format("/{0}/{1}/{2}{3}", "/images", databaseName, categoryDb.Slug, postDb.Slug);

        //                        if (!File.Exists(string.Format("{0}/{1}", pathSave, fileName)))
        //                        {
        //                            Helper.DownloadImage(pathAvatar, pathSave, fileName);
        //                        }
        //                        pathAvatar = string.Format("{0}/{1}", pathSave, fileName);

        //                        var fileFeatureName = Path.GetFileName(pathAvatar);
        //                        var feature = await wpClient.Media.CreateAsync(pathAvatar, fileFeatureName);
        //                        featureId = feature.Id;
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex?.Message);
        //                    }
        //                }

        //                var postWp = await wpClient.Posts.CreateAsync(new PostWp
        //                {
        //                    Title = new Title(postDb.Titlte),
        //                    Slug = postDb.Slug,
        //                    Categories = categoryIds,
        //                    Content = new Content(postDb.Description),
        //                    Status = Status.Publish,
        //                    //Tags = ,
        //                    FeaturedMedia = featureId,
        //                    TacGia = tacgiaIds,
        //                    Meta = new MetaWp
        //                    {
        //                        Source = metadataSource.Value.FirstOrDefault(),
        //                        Status = metadataStatus.Value.FirstOrDefault(),
        //                        AlternativeName = metadataAlternativeName.Value.Count > 1 ? metadataAlternativeName.Value[1] : metadataAlternativeName.Value.FirstOrDefault()
        //                    }
        //                });

        //                postLog = new PostLog
        //                {
        //                    CategorySlug = categoryDb.Slug,
        //                    PostWpId = postWp.Id,
        //                    Slug = postDb.Slug
        //                };
        //                await _postLogRepository.InsertOneAsync(postLog);
        //                Console.WriteLine(string.Format("POST SYNCED: Id: {0}, Slug: {0}, Title: {1}", postDb.Id, postDb.Slug, postDb.Titlte));
        //            }

        //            // sync chap
        //            await JobSyncChap(wpClient, postLog, postDb);
        //        }
        //    }
        //}

        //private async Task JobSyncChap(WordPressClient wpClient, PostLog postLog, PostDb postDb)
        //{
        //    var chapsLogs = _chapLogRepository.FilterBy(cwp => cwp.PostSlug == postDb.Slug).ToList();
        //    var chapsDbsNotSync = chapsLogs.Any() ?
        //                            _chapDbRepository.FilterBy(cdb => cdb.PostSlug == postDb.Slug && !chapsLogs.Any(cwp => cwp.Slug == cdb.Slug)).ToList()
        //                            : _chapDbRepository.FilterBy(cdb => cdb.PostSlug == postDb.Slug).ToList();
        //    if (chapsDbsNotSync.Any())
        //    {
        //        var newChapDbdsNotSync = chapsDbsNotSync.OrderBy(cdb => cdb.Index).ToList();
        //        for (var i = 0; i < newChapDbdsNotSync.Count(); i++)
        //        {
        //            var chapDb = newChapDbdsNotSync[i];
        //            if ((i + 1) != chapDb.Index) break;
        //            var newChapWp = new ChapWp
        //            {
        //                Content = new Content(chapDb.Content),
        //                Parent = postLog.PostWpId,
        //                Slug = chapDb.Slug,
        //                Type = "chap",
        //                Title = new Title(chapDb.Titlte),
        //                Status = Status.Publish
        //            };
        //            var chapWpSynced = await wpClient.CustomRequest.CreateAsync<ChapWp, ChapWp>("/wp-json/wp/v2/chap", newChapWp);
        //            var newChapLog = new ChapLog
        //            {
        //                Slug = chapDb.Slug,
        //                PostSlug = chapDb.PostSlug
        //            };
        //            await _chapLogRepository.InsertOneAsync(newChapLog);
        //            Console.WriteLine(string.Format("CHAP SYNCED: Slug: {0}, Title: {1}", chapDb.Slug, chapDb.Titlte));
        //        }
        //    }
        //}
    }
}