using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class CollectionEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("image1")]
        public string Thumbnail { get; set; }
        [JsonIgnore]
        public Nullable<int> Status { get; set; }
        [JsonIgnore]
        public string Url { get; set; }
        [JsonIgnore]
        public bool IsFeatured { get; set; }
    }
}
