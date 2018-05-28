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
    public class SongServices : ISongServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;

        #endregion

        #region constructors
        public SongServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region public functions

        public SongEntity GetSongById(int songId)
        {
            var song = _unitOfWork.SongRepository.GetByID(songId);
            if (song != null)
            {
                Mapper.CreateMap<ms_Song, SongEntity>();
                var songModel = Mapper.Map<ms_Song, SongEntity>(song);
                return songModel;
            }
            return null;
        }

        public IEnumerable<SongEntity> GetSongsByCategory(string categoryUrl)
        {
            ms_Genre foundGenre = _unitOfWork.GenreRepository.GetWithInclude(g => g.Url == categoryUrl, "Songs").FirstOrDefault();
            if (foundGenre == null) return null;
            if (foundGenre.Songs.Any())
            {
                Mapper.CreateMap<ms_Song, SongEntity>();
                var songsModel = Mapper.Map<List<ms_Song>, List<SongEntity>>(foundGenre.Songs.ToList());
                return songsModel;
            }
            return null;
        }

        public IEnumerable<SongEntity> GetTopSongs(int top)
        {
            var songs = _unitOfWork.SongRepository.GetAll().Take(top).ToList();
            if (songs.Any())
            {
                Mapper.CreateMap<ms_Song, SongEntity>();
                var songsModel = Mapper.Map<List<ms_Song>, List<SongEntity>>(songs);
                return songsModel;
            }
            return null;
        }

        public IEnumerable<SongEntity> GetFeaturedSongs()
        {
            var songs = _unitOfWork.SongRepository.GetManyQueryable(s => s.IsFeatured == true).ToList();
            if (songs.Any())
            {
                Mapper.CreateMap<ms_Song, SongEntity>();
                var songsModels = Mapper.Map<List<ms_Song>, List<SongEntity>>(songs.ToList());
                return songsModels;
            }
            return null;
        }

        public IEnumerable<SongEntity> SearchByName(string query)
        {
            var songs = _unitOfWork.SongRepository.GetMany(s => s.Title.Contains(query));
            if (songs.Any())
            {
                Mapper.CreateMap<ms_Song, SongEntity>();
                var songsModels = Mapper.Map<List<ms_Song>, List<SongEntity>>(songs.ToList());
                return songsModels;
            }
            return null;
        }

        #endregion
    }
}
