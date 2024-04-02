

namespace XLeech.Core.Model
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FeatureImage { get; set; }
        public string Parent { get; set; }
        /// <summary>
        /// Note
        /// </summary>
        public string Meta { get; set; }
    }
}
