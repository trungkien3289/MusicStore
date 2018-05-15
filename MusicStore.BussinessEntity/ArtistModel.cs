using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class ArtistEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Status { get; set; }
        public string Url { get; set; }
    }
}
