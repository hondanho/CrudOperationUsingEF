
using AngleSharp.Html.Dom;
using WordPressPCL.Models;
using XLeech.Core.Model;
using XLeech.Data.Entity;

namespace XLeech.Core
{
    public interface IProccessor
    {
        //Task<List<string>> GetPostUrls(IHtmlDocument document, SiteConfig siteConfig);

        CategoryModel GetCategory(IHtmlDocument document, SiteConfig siteConfig);
        Task<CategoryModel> IsExistCategory(CategoryModel post, SiteConfig siteConfig);
        Task<Category> SaveCategory(CategoryModel category);

        PostModel GetPost(IHtmlDocument document, SiteConfig siteConfig);
        Task<PostModel> IsExistPost(PostModel post, SiteConfig siteConfig);
        Task<bool> SavePost(PostModel post, List<int>? cateogoryIds);
    }
}
