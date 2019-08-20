using MusicStore.BussinessEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class GetFeaturedArtists
    {
        [JsonProperty("artists")]
        public List<ArtistEntity> Artists { get; set; }
    }

    public class GetTopSongsOfArtistResponse
    {
        [JsonProperty("songs")]
        public IList<SummarySongModel> Songs { get; set; }
    }
}