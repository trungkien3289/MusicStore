using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Services
{
    public class GenreServices : IGenreServices
    {
        private readonly UnitOfWork _unitOfWork;
        #region constructors
        public GenreServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion

        public IEnumerable<GenreEntity> GetAllGenres()
        {
            var genres = _unitOfWork.GenreRepository.GetAll().ToList();
            if (genres.Any())
            {
                Mapper.CreateMap<ms_Genre, GenreEntity>();
                var genresModel = Mapper.Map<List<ms_Genre>, List<GenreEntity>>(genres);
                return genresModel;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetAlbumsOfGenre(int id)
        {
            var genre = _unitOfWork.GenreRepository.GetSingleWithInclude(g => g.Id == id, "Albums");
            List<AlbumEntity> albumEntities = new List<AlbumEntity>();
            if (genre != null)
            {
                if (genre.Albums.Any())
                {
                    Mapper.CreateMap<ms_Album, AlbumEntity>();
                    albumEntities = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(genre.Albums.ToList());
                    return albumEntities;
                }
                else
                {
                    return albumEntities;
                }
            }
            return null;
        }

        public IEnumerable<SongEntity> GetSongsOfGenre(int id)
        {
            var genre = _unitOfWork.GenreRepository.GetSingleWithInclude(g => g.Id == id, "Songs");
            List<SongEntity> songEntities = new List<SongEntity>();
            if (genre != null)
            {
                if (genre.Songs.Any())
                {
                    Mapper.CreateMap<ms_Song, SongEntity>();
                    songEntities = Mapper.Map<List<ms_Song>, List<SongEntity>>(genre.Songs.ToList());
                    return songEntities;
                }
                else
                {
                    return songEntities;
                }
            }
            return null;
        }

        public IEnumerable<ArtistEntity> GetArtistsOfGenre(int id)
        {
            var genre = _unitOfWork.GenreRepository.GetSingleWithInclude(g => g.Id == id, "Artists");
            List<ArtistEntity> artistEntities = new List<ArtistEntity>();
            if (genre != null)
            {
                if (genre.Songs.Any())
                {
                    Mapper.CreateMap<ms_Artist, ArtistEntity>();
                    artistEntities = Mapper.Map<List<ms_Artist>, List<ArtistEntity>>(genre.Artists.ToList());
                    return artistEntities;
                }
                else
                {
                    return artistEntities;
                }
            }
            return null;
        }

    }
}
