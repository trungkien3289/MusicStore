using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class GenreEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("genre")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonIgnore]
        public Nullable<int> Status { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("image1")]
        public string Thumbnail { get; set; }
    }

    public class GenreDetails
    {
        [JsonProperty("artists")]
        public IList<ArtistEntity> Artists { get; set; }
        [JsonProperty("songs")]
        public IList<SongEntity> Songs { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
