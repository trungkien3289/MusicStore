using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class ApplicationEntity
    {
        [JsonProperty("applicationId")]
        public int Id { get; set; }
        [JsonProperty("id")]
        public string AppId { get; set; }
        [JsonProperty("wdgid")]
        public string WDGId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("generic")]
        public bool Generic { get; set; }
    }
}
