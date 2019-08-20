using MusicStore.BussinessEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class GetSongsAndArtistsOfGenreResponse
    {
        public GetSongsAndArtistsOfGenreResponse()
        {
            Artists = new List<ArtistEntity>();
            Songs = new List<SongEntity>();
        }
        [JsonProperty("artists")]
        public IList<ArtistEntity> Artists { get; set; }
        [JsonProperty("songs")]
        public IList<SongEntity> Songs { get; set; }
    }

    public class GetAllGenresResponse
    {
        [JsonProperty("genres")]
        public IList<GenreEntity> Genres { get; set; }
    }
}