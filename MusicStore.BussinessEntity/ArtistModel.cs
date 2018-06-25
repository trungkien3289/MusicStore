using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class ArtistEntity
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
        [JsonIgnore]
        public bool IsFeatured { get; set; }
        [JsonProperty("songs")]
        public int NumberOfSongs { get; set; }
    }

    public class ArtistDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public int NumberOfSongs { get; set; }
        public IList<SongEntity> Songs { get; set; }
    }
}
