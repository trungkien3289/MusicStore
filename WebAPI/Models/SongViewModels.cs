using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class GetFeaturedSongs
    {
        [JsonProperty("songs")]
        public IList<SummarySongModel> Songs { get; set; }
    }

    public class ShortSummarySongModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("album")]
        public string AlbumName { get; set; }
        [JsonProperty("song")]
        public string Title { get; set; }
        [JsonProperty("image1")]
        public string Thumbnail { get; set; }
        [JsonProperty("mp3")]
        public string MediaUrl { get; set; }
        [JsonProperty("lyrics")]
        public string Lyrics { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("duration")]
        public Nullable<double> Duration { get; set; }
        [JsonProperty("bitrate")]
        public Nullable<double> Quality { get; set; }
        [JsonProperty("artist")]
        public string ArtistName { get; set; }
    }

    public class GetSongDetailsResponse
    {
        public GetSongDetailsResponse()
        {
            Songs = new List<ShortSummarySongModel>();
        }
        [JsonProperty("songs")]
        public IList<ShortSummarySongModel> Songs { get; set; }
    }
}