using System.ComponentModel.DataAnnotations;

namespace XLeech.Core.Model
{
    public class PostModel
    {
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Đoạn trích
        /// </summary>
        public string? Excerpt { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Slug { get; set; }

        public string? FeatureImage { get; set; }

        [Required]
        /// <summary>
        /// standard | gallery | video
        /// </summary>
        public string Format { get; set; }

        [Required]
        /// <summary>
        /// publish | draft
        /// </summary>
        public string Status { get; set; }

        [Required]
        /// <summary>
        /// post | page | attachment
        /// </summary>
        public string Type { get; set; }

        public string? Author { get; set; }

        public List<string> Tags { get; set; }

        public string? CategoryName { get; set; }

        public DateTime? PostDate { get; set; }

        public List<string> MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        public string Meta {  get; set; }

    }
}
