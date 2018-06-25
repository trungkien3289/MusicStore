using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class CollectionViewModels
    {
    }

    public class GetSongsOfCollectionResponse
    {
        [JsonProperty("songs")]
        public IList<SummarySongModel> Songs { get; set; }
    }

    public class CollectionSummary
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Title { get; set; }
        [JsonProperty("image1")]
        public string Thumbnail { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

    }

    public class GetFeaturedCollections
    {
        [JsonProperty("playlists")]
        public IList<CollectionSummary> Collections { get; set; }
    }
}