using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class ms_Artist
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Status { get; set; }
        public string Url { get; set; }

        public virtual ICollection<ms_Song> Songs { get; set; }
        public virtual ICollection<ms_Album> Albums { get; set; }
    }
}
