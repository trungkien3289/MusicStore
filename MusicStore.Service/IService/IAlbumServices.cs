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
        AlbumEntity GetAlbumWithArtists(int albumId);
        IEnumerable<SongEntity> GetSongsOfAlbum(int albumId);
        IEnumerable<AlbumEntity> GetAllAlbums();
        IEnumerable<AlbumEntity> GetFeaturedAlbums();
        int CreateAlbum(AlbumEntity albumSummary);
        bool UpdateAlbum(int albumId, AlbumEntity albumSummary);
        bool DeleteAlbum(int albumId);
        IEnumerable<AlbumEntity> GetTopAlbums(int top);
        IEnumerable<AlbumEntity> GetAlbumsByCategory(string categoryUrl);
        IEnumerable<AlbumEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 10);
        IEnumerable<AlbumEntity> GetAlbumsOfArtist(int artistId, int top);
        IEnumerable<AlbumEntity> GetAlbumsHasSameArtists(int albumId);
        IEnumerable<AlbumEntity> GetAlbumsHasSameGenres(int albumId);
        IEnumerable<AlbumEntity> GetAlbumsAfterBeginCharacter(string character, int page, int pagesize);
    }
}
