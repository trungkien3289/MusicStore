using MusicStore.BussinessEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            TopAlbums = new List<AlbumSummary>();
        }
        public IList<AlbumSummary> TopAlbums { get; set; }
    }

    public class GetListApplicationsResponse
    {
        [JsonProperty("ads")]
        public IList<ApplicationEntity> Apllications { get; set; }
    }

}