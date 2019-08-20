using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models
{
    public class SummarySongModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "imageurl")]
        public string Thumbnail { get; set; }
        [JsonProperty(PropertyName = "file")]
        public string MediaUrl { get; set; }
    }
}