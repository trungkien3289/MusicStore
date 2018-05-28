using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class ms_Album
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public Nullable<DateTime> ReleaseDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string Url { get; set; }
        public bool IsFeatured { get; set; }


        public virtual ICollection<ms_Genre> Genres { get; set; }
        public virtual ICollection<ms_Artist> Artists { get; set; }
        public virtual ICollection<ms_Song> Songs { get; set; }
    }
}
