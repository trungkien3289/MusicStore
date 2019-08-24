using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                var mapper = config.CreateMapper();
                var albumModel = mapper.Map<ms_Album, AlbumEntity>(album);
                return albumModel;
            }
            return null;
        }

        public AlbumEntity GetAlbumWithArtists(int albumId)
        {
            var album = _unitOfWork.AlbumRepository.GetSingleWithInclude(a => a.Id == albumId, "Artists");
            if (album != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                var mapper = config.CreateMapper();
                var albumModel = mapper.Map<ms_Album, AlbumEntity>(album);
                albumModel.ArtistName = String.Join(" - ", album.Artists.Select(a => a.Name).ToList());
                return albumModel;
            }
            return null;
        }

        public IEnumerable<SongEntity> GetSongsOfAlbum(int albumId)
        {
            var album = _unitOfWork.AlbumRepository.GetWithInclude(a => a.Id == albumId, "Songs").FirstOrDefault();
            if (album != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
                .ForMember(ae => ae.AlbumId, map => map.MapFrom(albs => album.Id))
                    .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => album.Title))
                    .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => album.Thumbnail))
                    .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Id : 0))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty))
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Thumbnail : String.Empty)));
                var mapper = config.CreateMapper();
                var songs = mapper.Map<List<ms_Song>, List<SongEntity>>(album.Songs.ToList());
                return songs;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetAllAlbums()
        {
            var albums = _unitOfWork.AlbumRepository.GetAll().ToList();
            if (albums.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                var mapper = config.CreateMapper();
                var albumsModel = mapper.Map<List<ms_Album>, List<AlbumEntity>>(albums);
                return albumsModel;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetTopAlbums(int top)
        {
            var albums = _unitOfWork.AlbumRepository.GetAll().Take(top).ToList();
            if (albums.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                var mapper = config.CreateMapper();
                var albumsModel = mapper.Map<List<ms_Album>, List<AlbumEntity>>(albums);
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
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                var mapper = config.CreateMapper();
                var albumsModel = mapper.Map<List<ms_Album>, List<AlbumEntity>>(foundGenre.Albums.ToList());
                return albumsModel;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetFeaturedAlbums()
        {
            var featuredAlbums = _unitOfWork.AlbumRepository.GetMany(a => a.IsFeatured == true).ToList();
            if (featuredAlbums.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                var mapper = config.CreateMapper();
                var albums = mapper.Map<List<ms_Album>, List<AlbumEntity>>(featuredAlbums);
                return albums;
            }

            return null;
        }

        public IEnumerable<AlbumEntity> GetAlbumsOfArtist(int artistId, int top = 0)
        {
            var artist = _unitOfWork.ArtistRepository.GetSingleWithInclude(a => a.Id == artistId, "Albums", "Albums.Songs");
            IList<AlbumEntity> result = new List<AlbumEntity>();
            if (artist != null && artist.Albums.Any())
            {
                var listAlbums = artist.Albums.ToList();
                if (top != 0)
                {
                    listAlbums = listAlbums.Take((int)top).ToList();
                }
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>()
                .ForMember(ae => ae.NumberOfSong, map => map.MapFrom(albs => albs.Songs.ToList().Count()))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => artist.Name)));
                var mapper = config.CreateMapper();

                result = mapper.Map<List<ms_Album>, List<AlbumEntity>>(listAlbums);
            }

            return result;
        }

        public IEnumerable<AlbumEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 10)
        {
            var albums = _unitOfWork.AlbumRepository.GetWithInclude(a => a.Title.Contains(query)).OrderBy(a => a.Title).Skip(--page * numberItemsPerPage).Take(numberItemsPerPage).ToList();
            if (albums.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                var mapper = config.CreateMapper();
                var albumEntities = mapper.Map<List<ms_Album>, List<AlbumEntity>>(albums);
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

        public IEnumerable<AlbumEntity> GetAlbumsHasSameArtists(int albumId)
        {
            var foundAlbum = _unitOfWork.AlbumRepository.GetSingleWithInclude(a => a.Id == albumId, "Artists");
            if (foundAlbum != null)
            {
                var artistIds = foundAlbum.Artists.Select(a => a.Id).ToArray();
                var artists = this._unitOfWork.ArtistRepository.GetWithInclude(a => artistIds.Contains(a.Id), "Albums").ToList();
                var returnAlbums = artists.SelectMany(a => a.Albums).Distinct<ms_Album>().ToList();
                if (returnAlbums.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                    var mapper = config.CreateMapper();
                    var albumEntities = mapper.Map<List<ms_Album>, List<AlbumEntity>>(returnAlbums);
                    return albumEntities;
                }
            }

            return null;
        }

        public IEnumerable<AlbumEntity> GetAlbumsHasSameGenres(int albumId)
        {
            var foundAlbum = _unitOfWork.AlbumRepository.GetSingleWithInclude(a => a.Id == albumId, "Genres");
            if (foundAlbum != null)
            {
                IList<int> genreIds = foundAlbum.Genres.Select(a => a.Id).ToList();
                var genres = this._unitOfWork.GenreRepository.GetWithInclude(a => genreIds.Contains(a.Id), "Albums").ToList();
                var returnAlbums = genres.SelectMany(a => a.Albums).Distinct<ms_Album>().ToList();
                if (returnAlbums.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                    var mapper = config.CreateMapper();
                    var albumEntities = mapper.Map<List<ms_Album>, List<AlbumEntity>>(returnAlbums);
                    return albumEntities;
                }
            }

            return null;
        }

        public IEnumerable<AlbumEntity> GetAlbumsAfterBeginCharacter(string character,int page, int pagesize)
        {
            IList<AlbumEntity> result = new List<AlbumEntity>();
            IList<ms_Album> albums;
            if (character.Trim().Equals("0-9"))
            {
                albums = _unitOfWork.AlbumRepository.GetWithInclude(a => Regex.IsMatch(a.Title, @"^\d"), "Artists", "Songs").OrderBy(a => a.Title).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
            else
            {
                albums = _unitOfWork.AlbumRepository.GetWithInclude(a => a.Title.StartsWith(character), "Artists", "Songs").OrderBy(a => a.Title).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
            if (albums != null && albums.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>()
                .ForMember(a => a.NumberOfSong, map => map.MapFrom(albs => albs.Songs.Count()))
                .ForMember(a => a.ArtistName, map => map.MapFrom(albs => String.Join(" - ", albs.Artists.Select(a => a.Name).ToList()))));
                var mapper = config.CreateMapper();
                result = mapper.Map<IList<ms_Album>, IList<AlbumEntity>>(albums);
                return result;
            }

            return result;
        }
        #endregion
    }
}
