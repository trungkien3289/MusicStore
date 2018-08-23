using MusicStore.BussinessEntity;
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

    public class ArtistViewResponse
    {
        public ArtistViewResponse()
        {
            Artists = new List<ArtistEntity>();
            FeaturedArtists = new List<ArtistEntity>();
            FeaturedAlbums = new List<AlbumSummary>();
        }
        public IList<ArtistEntity> Artists { get; set; }
        public IList<ArtistEntity> FeaturedArtists { get; set; }
        public IList<AlbumSummary> FeaturedAlbums { get; set; }
    }

    public class ArtistDetailsViewModel
    {
        public ArtistDetailsViewModel()
        {
            Albums = new List<AlbumSummary>();
            Songs = new List<SongEntity>();
            RelatedArtists = new List<ArtistEntity>();
        }

        public ArtistEntity Details { get; set; }
        public IList<AlbumSummary> Albums { get; set; }
        public IList<SongEntity> Songs { get; set; }
        public IList<ArtistEntity> RelatedArtists { get; set; }
    }
}