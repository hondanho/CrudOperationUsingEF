using System.ComponentModel.DataAnnotations;

namespace XAutoLeech.Database.Model
{
    public class Category
    {
        public int Id { get; set; }
        public int SiteID { get; set; }
        public string CategoryListPageURL { get; set; }
        public string CategoryListURLSelector { get; set; }
        public string CategoryMap { get; set; }
        public string CategoryPostURLSelector { get; set; }
        public string CategoryNextPageURLSelector { get; set; }
        public bool SaveFeaturedImages { get; set; }
        public string FeaturedImageSelector { get; set; }
        public string FindDndReplaceRawHTML { get; set; }
        public string RemoveElementAttributes { get; set; }
        public string UnnecessaryElements { get; set; }
    }
}