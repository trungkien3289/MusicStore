using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class AlbumViewModel
    {
        public AlbumViewModel()
        {
            AlbumsSameArtists = new List<AlbumSummary>();
            AlbumsSameGenres = new List<AlbumSummary>();
            Songs = new List<SongEntity>();
        }
        public IList<AlbumSummary> AlbumsSameArtists { get; set; }
        public IList<AlbumSummary> AlbumsSameGenres { get; set; }
        public IList<SongEntity> Songs { get; set; }
    }
}