
namespace XLeech.Data.Entity
{
    public class Category: BaseEntity
    {
        public int SiteID { get; set; }
        public string? CategoryListPageURL { get; set; }
        public string? CategoryListURLSelector { get; set; }
        public string? CategoryPostURLSelector { get; set; }
        public string? CategoryNextPageURLSelector { get; set; }
        public string? Urls { get; set; }
        public string? CategoryMap { get; set; }
        public bool SaveFeaturedImages { get; set; }
        public string? FeaturedImageSelector { get; set; }
        public string? FindAndReplaceRawHTML { get; set; }
        public string? RemoveElementAttributes { get; set; }
        public string? UnnecessaryElements { get; set; }
    }
}