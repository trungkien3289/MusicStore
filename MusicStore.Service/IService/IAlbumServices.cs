using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IServices
{
    public interface IAlbumServices
    {
        AlbumEntity GetAlbumById(int albumId);
        IEnumerable<AlbumEntity> GetAllAlbums();
        int CreateAlbum(AlbumEntity albumSummary);
        bool UpdateAlbum(int albumId, AlbumEntity albumSummary);
        bool DeleteAlbum(int albumId);
    }
}
