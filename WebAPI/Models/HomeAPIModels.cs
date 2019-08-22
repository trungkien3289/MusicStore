using MusicStore.BussinessEntity;
using MusicStore.BussinessEntity.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class SearchResultEntity
    {
        [JsonProperty("type")]
        public ItemType Type { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }

    public class SearchResult
    {
        [JsonProperty("albums")]
        public IList<AlbumEntity> Albums { get; set; }
        [JsonProperty("songs")]
        public IList<SongEntity> Songs { get; set; }
        [JsonProperty("artists")]
        public IList<ArtistEntity> Artists { get; set; }
    }

    public class WebSearchResult
    {
        [JsonProperty("albums")]
        public IList<AlbumEntity> Albums { get; set; }
        [JsonProperty("songs")]
        public IList<SongEntity> Songs { get; set; }
        [JsonProperty("artists")]
        public IList<ArtistEntity> Artists { get; set; }
        public IList<ArtistEntity> TopArtists { get; set; }
    }
}