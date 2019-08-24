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
            var song = _unitOfWork.SongRepository.GetWithInclude(s => s.Id == songId, "Albums", "Artists").FirstOrDefault();
            if (song != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Thumbnail : String.Empty))
                    .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Title : String.Empty))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty)));
                var mapper = config.CreateMapper();
                var songModel = mapper.Map<ms_Song, SongEntity>(song);
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
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Thumbnail : String.Empty))
                .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Title : String.Empty))
                .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty)));
                var mapper = config.CreateMapper();
                var songsModel = mapper.Map<List<ms_Song>, List<SongEntity>>(foundGenre.Songs.ToList());
                return songsModel;
            }
            return null;
        }

        public IEnumerable<SongEntity> GetTopSongs(int top)
        {
            var songs = _unitOfWork.SongRepository.GetWithInclude(s => true, "Albums", "Artists").Take(top).ToList();
            if (songs != null && songs.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
                    .ForMember(ae => ae.AlbumId, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Id : 0))
                    .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Title : String.Empty))
                    .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Thumbnail : String.Empty))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty))
                    .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Id : 0)));
                var mapper = config.CreateMapper();
                var songsModel = mapper.Map<List<ms_Song>, List<SongEntity>>(songs.ToList());
                return songsModel;
            }
            return null;
        }

        public IEnumerable<SongEntity> GetFeaturedSongs()
        {
            var songs = _unitOfWork.SongRepository.GetWithInclude(s => s.IsFeatured == true, "Albums", "Artists").ToList();
            if (songs.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
               .ForMember(ae => ae.AlbumId, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Id : 0))
               .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Title : String.Empty))
               .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Thumbnail : String.Empty))
               .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty))
               .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Id : 0)));
                var mapper = config.CreateMapper();
                var songsModels = mapper.Map<List<ms_Song>, List<SongEntity>>(songs.ToList());
                return songsModels;
            }
            return null;
        }

        public IEnumerable<SongEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 20)
        {
            if (page > 0 && numberItemsPerPage > 0)
            {
                var songs = _unitOfWork.SongRepository.GetWithInclude(s => s.Title.Contains(query), "Albums", "Artists").OrderBy(s => s.Title).Skip(--page * numberItemsPerPage).Take(numberItemsPerPage).ToList();
                if (songs.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
                        .ForMember(ae => ae.AlbumId, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Id : 0))
                        .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Title : String.Empty))
                        .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => albs.Albums.Count() > 0 ? albs.Albums.First().Thumbnail : String.Empty))
                        .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty))
                        .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Id : 0)));
                    var mapper = config.CreateMapper();
                    var songsModels = mapper.Map<List<ms_Song>, List<SongEntity>>(songs.ToList());
                    return songsModels;
                }
            }

            return null;
        }

        #endregion
    }
}
