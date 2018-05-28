using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class ms_Song
    {
        [Key]
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string MediaUrl { get; set; }
        public string Lyrics { get; set; }
        public Nullable<int> Status { get; set; }
        public string Url { get; set; }
        public Nullable<double> Duration { get; set; }
        public bool IsFeatured { get; set; }

        public virtual ICollection<ms_Genre> Genres { get; set; }
        public virtual ICollection<ms_Collection> Collections { get; set; }
        public virtual ICollection<ms_Artist> Artists { get; set; }
        public virtual ICollection<ms_Album> Albums { get; set; }
    }
}
