using Newtonsoft.Json;
using XLeech.Data.Utils;

namespace XLeech.Data.Entity.Wordpress
{
    public class MetaWp
    {
        [JsonProperty(MetaFieldPost.Source, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty(MetaFieldPost.Status, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(MetaFieldPost.AlternativeName, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AlternativeName { get; set; }
    }
}
