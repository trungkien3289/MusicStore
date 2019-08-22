using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class GenreViewModel
    {
        public GenreViewModel()
        {
            Artists = new List<ArtistEntity>();
            Songs = new List<SongEntity>();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<ArtistEntity> Artists { get; set; }
        public IList<SongEntity> Songs { get; set; }
    }
}