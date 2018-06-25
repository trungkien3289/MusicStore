using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MusicStore.Service.Services
{
    public class AlbumServices : IAlbumServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;

        #endregion

        #region constructors
        public AlbumServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region public functions
        public AlbumEntity GetAlbumById(int albumId)
        {
            var album = _unitOfWork.AlbumRepository.GetByID(albumId);
            if (album != null)
            {
                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albumModel = Mapper.Map<ms_Album, AlbumEntity>(album);
                return albumModel;
            }
            return null;
        }

        public IEnumerable<SongEntity> GetSongsOfAlbum(int albumId)
        {
            var album = _unitOfWork.AlbumRepository.GetWithInclude(a => a.Id == albumId, "Songs").FirstOrDefault();
            if (album != null)
            {
                Mapper.CreateMap<ms_Song, SongEntity>()
                    .ForMember(ae => ae.AlbumId, map => map.MapFrom(albs => album.Id))
                    .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => album.Title))
                    .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => album.Thumbnail))
                    .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Id : 0))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty))
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Thumbnail : String.Empty));
                var songs = Mapper.Map<List<ms_Song>, List<SongEntity>>(album.Songs.ToList());
                return songs;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetAllAlbums()
        {

            var albums = _unitOfWork.AlbumRepository.GetAll().ToList();
            if (albums.Any())
            {
                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albumsModel = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(albums);
                return albumsModel;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetTopAlbums(int top)
        {
            var albums = _unitOfWork.AlbumRepository.GetAll().Take(top).ToList();
            if (albums.Any())
            {
                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albumsModel = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(albums);
                return albumsModel;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetAlbumsByCategory(string categoryUrl)
        {
            ms_Genre foundGenre = _unitOfWork.GenreRepository.GetWithInclude(g => g.Url == categoryUrl, "Albums").FirstOrDefault();
            if (foundGenre == null) return null;
            if (foundGenre.Albums.Any())
            {
                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albumsModel = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(foundGenre.Albums.ToList());
                return albumsModel;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetFeaturedAlbums()
        {
            var featuredAlbums = _unitOfWork.AlbumRepository.GetMany(a => a.IsFeatured == true).ToList();
            if(featuredAlbums.Any())
            {
                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albums = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(featuredAlbums);
                return albums;
            }

            return null;
        }

        public IEnumerable<AlbumEntity> GetAlbumsOfArtist(int artistId, int top = 0)
        {
            var artist = _unitOfWork.ArtistRepository.GetSingleWithInclude(a => a.Id == artistId, "Albums", "Albums.Songs");
            IList<AlbumEntity> result = new List<AlbumEntity>();
            if (artist!=null && artist.Albums.Any())
            {
                var listAlbums = artist.Albums.ToList();
                if (top != 0)
                {
                    listAlbums = listAlbums.Take((int)top).ToList();
                }
                Mapper.CreateMap<ms_Album, AlbumEntity>()
                    .ForMember(ae => ae.NumberOfSong, map => map.MapFrom(albs => albs.Songs.ToList().Count()))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => artist.Name));
                result = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(listAlbums);
            }

            return result;
        }

        public IEnumerable<AlbumEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 10)
        {
            var albums = _unitOfWork.AlbumRepository.GetManyQueryable(a => a.Title.Contains(query)).OrderBy(a =>a.Title).Skip(--page*numberItemsPerPage).Take(numberItemsPerPage).ToList();
            if (albums.Any())
            {
                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albumEntities = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(albums);
                return albumEntities;
            }

            return null;
        }

        public int CreateAlbum(AlbumEntity albumSummary)
        {
            using (var scope = new TransactionScope())
            {
                var album = new ms_Album()
                {
                    Title = albumSummary.Title
                };
                _unitOfWork.AlbumRepository.Insert(album);
                _unitOfWork.Save();
                scope.Complete();

                return album.Id;
            }
        }

        public bool UpdateAlbum(int albumId, AlbumEntity albumSummary)
        {
            var success = false;
            if (albumSummary != null)
            {
                using (var scope = new TransactionScope())
                {
                    var album = _unitOfWork.AlbumRepository.GetByID(albumId);
                    album.Title = albumSummary.Title;
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
            }

            return success;
        }

        public bool DeleteAlbum(int albumId)
        {
            var success = false;
            if(albumId>0){
                using (var scope = new TransactionScope())
                {
                    var album = _unitOfWork.AlbumRepository.GetByID(albumId);
                    if (album != null)
                    {
                        _unitOfWork.AlbumRepository.Delete(album);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }

            return success;
        }


        #endregion
    }
}
