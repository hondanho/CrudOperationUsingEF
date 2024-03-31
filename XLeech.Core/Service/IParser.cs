using AngleSharp.Html.Dom;
using XLeech.Core.Model;

namespace XLeech.Core.Service
{
    public interface IParser
    {
        Task<CategoryModel> GetCategory(IHtmlDocument document);
        Task<PostModel> GetPost(IHtmlDocument document);
    }
}
