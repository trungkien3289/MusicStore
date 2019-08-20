using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class AlbumSummary
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Title { get; set; }
        [JsonProperty("image1")]
        public string Thumbnail { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        public string ReleaseDate { get; set; }

    }

    public class AlbumShortSummary
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("album")]
        public string Title { get; set; }
        [JsonProperty("image1")]
        public string Thumbnail { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("artist")]
        public string ArtistName { get; set; }
        [JsonProperty("releaseDate")]
        public string ReleaseDate { get; set; }
        [JsonProperty("songs")]
        public int NumberOfSong { get; set; }
    }

    public class GetFeaturedAlbums
    {
        [JsonProperty("playlists")]
        public IList<AlbumSummary> Albums { get; set; }
    }

    public class GetSongsOfAlbumResponse
    {
        [JsonProperty("songs")]
        public IList<SummarySongModel> Songs { get; set; }
    }

    public class GetAlbumsOfArtistResponse
    {
        [JsonProperty("albums")]
        public IList<AlbumShortSummary> Albums { get; set; }
    }

}