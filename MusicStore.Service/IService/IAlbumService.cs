using MusicStore.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IService
{
    public interface IAlbumService
    {
        AlbumSummary GetAlbumById(int albumId);
        IEnumerable<AlbumSummary> GetAllAlbums();
        int CreateAlbum(AlbumSummary albumSummary);
        bool UpdateAlbum(int albumId, AlbumSummary albumSummary);
        bool DeleteAlbum(int albumId);
    }
}
