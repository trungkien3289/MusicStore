using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class SongEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("albumid")]
        public int AlbumId { get; set; }
        [JsonProperty("album")]
        public string AlbumName { get; set; }
        [JsonProperty("albumImage1")]
        public string AlbumThumbnail { get; set; }
        [JsonProperty("song")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("image1")]
        public string Thumbnail { get; set; }
        [JsonProperty("mp3")]
        public string MediaUrl { get; set; }
        [JsonProperty("lyrics")]
        public string Lyrics { get; set; }
        [JsonIgnore]
        public Nullable<int> Status { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("duration")]
        public Nullable<double> Duration { get; set; }
        [JsonProperty("bitrate")]
        public Nullable<double> Quality { get; set; }
        [JsonIgnore]
        public bool IsFeatured { get; set; }
        [JsonProperty("artist")]
        public string ArtistName { get; set; }
        [JsonProperty("artistId")]
        public string ArtistId { get; set; }
    }
}
