using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ArtistSummaryModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("artist")]
        public string Name { get; set; }
        [JsonIgnore]
        public Nullable<int> Status { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("image1")]
        public string Thumbnail { get; set; }
    }
}