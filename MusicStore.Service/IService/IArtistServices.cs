using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IService
{
    public interface IArtistServices
    {
        ArtistEntity GetArtistById(int artistId);
        IEnumerable<ArtistEntity> GetAllArtists();
        IEnumerable<ArtistEntity> GetTopArtists(int top);
        IEnumerable<ArtistEntity> GetArtistsByCategory(string categoryUrl);
        IEnumerable<ArtistEntity> GetFeaturedArtists();
        IEnumerable<SongEntity> GetSongsOfArtist(int id);
        IEnumerable<AlbumEntity> GetAlbumsOfArtist(int id);
        IEnumerable<ArtistEntity> SearchByName(string query);
    }
}
