
using AngleSharp.Html.Dom;
using XLeech.Core.Model;
using XLeech.Data.Entity;

namespace XLeech.Core
{
    public interface IProccessor
    {
        Task<CategoryModel> GetCategory(IHtmlDocument document, SiteConfig siteConfig);
        Task<PostModel> GetPost(IHtmlDocument document, SiteConfig siteConfig);
        Task<bool> IsExistCategory(CategoryModel post, SiteConfig siteConfig);
        Task<bool> IsExistPost(CategoryModel post, SiteConfig siteConfig);
        Task<bool> SavePost(PostModel post);
        Task<bool> SaveCategory(CategoryModel category);
    }
}
