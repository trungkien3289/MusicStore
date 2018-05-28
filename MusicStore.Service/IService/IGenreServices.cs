using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IService
{
    public interface IGenreServices
    {
        IEnumerable<GenreEntity> GetAllGenres();
        IEnumerable<AlbumEntity> GetAlbumsOfGenre(int id);
        IEnumerable<SongEntity> GetSongsOfGenre(int id);
        IEnumerable<ArtistEntity> GetArtistsOfGenre(int id);
    }
}
