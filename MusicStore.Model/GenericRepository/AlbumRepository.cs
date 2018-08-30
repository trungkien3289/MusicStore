using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.GenericRepository
{
    public class AlbumRepository : GenericRepository<ms_Album>
    {
        public AlbumRepository(DbContext context) : base(context)
        {
        }
    }
}
