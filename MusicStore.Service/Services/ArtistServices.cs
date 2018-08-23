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

namespace MusicStore.Service.Services
{
    public class ArtistServices : IArtistServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;
        #endregion

        #region constructors
        public ArtistServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region public functions
        public IEnumerable<ArtistEntity> GetAllArtists()
        {
            var artists = _unitOfWork.ArtistRepository.GetWithInclude(null, "Songs").ToList();
            if (artists.Any())
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>()
                    .ForMember(ae => ae.NumberOfSongs, map => map.MapFrom(artist => artist.Songs.Count()));
                var artistList = Mapper.Map<List<ms_Artist>,List<ArtistEntity>>(artists);
                return artistList;
            }
            return null;
        }

        public ArtistEntity GetArtistById(int artistId)
        {
            var artist = _unitOfWork.ArtistRepository.GetByID(artistId);
            if(artist != null)
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>();
                var artistEntity = Mapper.Map<ms_Artist, ArtistEntity>(artist);
                return artistEntity;
            }
            return null;
        }

        public IEnumerable<ArtistEntity> GetArtistsByCategory(string categoryUrl)
        {
            ms_Genre foundGenre = _unitOfWork.GenreRepository.GetWithInclude(g => g.Url == categoryUrl, "Artists").FirstOrDefault();
            if (foundGenre == null) return null;
            if (foundGenre.Artists.Any())
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>();
                var artistsModel = Mapper.Map<List<ms_Artist>, List<ArtistEntity>>(foundGenre.Artists.ToList());
                return artistsModel;
            }
            return null;
        }

        public IEnumerable<ArtistEntity> GetTopArtists(int top)
        {
            var artists = _unitOfWork.ArtistRepository.GetWithInclude(null, "Songs").Take(top).ToList();
            if (artists.Any())
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>()
                    .ForMember(ae => ae.NumberOfSongs, map => map.MapFrom(artist => artist.Songs.Count()));
                var artistList = Mapper.Map<List<ms_Artist>, List<ArtistEntity>>(artists);
                return artistList;
            }
            return null;
        }

        public IEnumerable<ArtistEntity> GetFeaturedArtists()
        {
            var artists = _unitOfWork.ArtistRepository.GetWithInclude(a => a.IsFeatured == true, "Songs").ToList();
            if (artists.Any())
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>()
                    .ForMember(ae => ae.NumberOfSongs, map => map.MapFrom(artist => artist.Songs.Count()));
                var artistList = Mapper.Map<List<ms_Artist>, List<ArtistEntity>>(artists);
                return artistList;
            }
            return null;
        }

        public IEnumerable<SongEntity> GetSongsOfArtist(int id)
        {
            var artist = _unitOfWork.ArtistRepository.GetWithInclude(a => a.Id == id, "Songs", "Songs.Albums").FirstOrDefault();
            if (artist != null)
            {
                IList<SongEntity> songs = new List<SongEntity>();

                Mapper.CreateMap<ms_Song, SongEntity>()
                    .ForMember(ae => ae.AlbumId, map => map.MapFrom(s => s.Albums.Count() > 0 ? s.Albums.First().Id : 0))
                    .ForMember(ae => ae.AlbumName, map => map.MapFrom(s => s.Albums.Count() > 0 ? s.Albums.First().Title : String.Empty))
                    .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(s => s.Albums.Count() > 0 ? s.Albums.First().Thumbnail : String.Empty))
                    .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => artist.Id))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => artist.Name))
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => artist.Thumbnail));
                var artistList = Mapper.Map<List<ms_Song>, List<SongEntity>>(artist.Songs.ToList());
                return artistList;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetAlbumsOfArtist(int id)
        {
            var artist = _unitOfWork.ArtistRepository.GetWithInclude(a => a.Id == id, "Albums").FirstOrDefault();
            if (artist != null)
            {
                IList<AlbumEntity> albums = new List<AlbumEntity>();

                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albumEntitys = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(artist.Albums.ToList());
                return albumEntitys;
            }
            return null;
        }

        public IEnumerable<ArtistEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 10)
        {
            var artists = _unitOfWork.ArtistRepository.GetWithInclude(a => a.Name.Contains(query), "Songs").OrderBy(a => a.Name).Skip(--page*numberItemsPerPage).Take(numberItemsPerPage).ToList();
            if (artists.Any())
            {
                Mapper.CreateMap<ms_Artist, ArtistEntity>()
                    .ForMember(ae => ae.NumberOfSongs, map => map.MapFrom(albs => albs.Songs.Count()));
                var artistList = Mapper.Map<List<ms_Artist>, List<ArtistEntity>>(artists);
                return artistList;
            }
            return null;
        }

        public IEnumerable<ArtistEntity> GetArtistsAfterBeginCharacter(string character, int page, int pagesize)
        {
            IList<ArtistEntity> result = new List<ArtistEntity>();
            IList<ms_Artist> artists;
            if (character.Trim().Equals("0-9"))
            {
                artists = _unitOfWork.ArtistRepository.GetWithInclude(a => Regex.IsMatch(a.Name, @"^\d"), "Albums", "Songs").OrderBy(a => a.Name).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
            else
            {
                artists = _unitOfWork.ArtistRepository.GetWithInclude(a => a.Name.StartsWith(character), "Albums", "Songs").OrderBy(a => a.Name).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
            if (artists != null && artists.Any())
            {
                for (int i = 0; i < artists.Count(); i++)
                {
                    var returnArtist = new ArtistEntity()
                    {
                        Id = artists[i].Id,
                        IsFeatured = artists[i].IsFeatured,
                        Name = artists[i].Name,
                        NumberOfAlbums = artists[i].Albums.Count(),
                        NumberOfSongs = artists[i].Songs.Count(),
                        Status = artists[i].Status,
                        Thumbnail = artists[i].Thumbnail,
                        Url = artists[i].Url
                    };

                    result.Add(returnArtist);
                }
                //Mapper.CreateMap<ms_Artist, ArtistEntity>()
                //.ForMember(a => a.NumberOfSongs, map => map.MapFrom(albs => albs.Songs.Count()))
                //.ForMember(a => a.NumberOfAlbums, map => map.MapFrom(albs => albs.Albums.Count()));
                // result = Mapper.Map<IList<ms_Artist>, IList<ArtistEntity>>(artists);
                return result;
            }

            return result;
        }

        public IEnumerable<ArtistEntity> GetArtistsHasSameGenre(int artistId)
        {
            var foundArtist = _unitOfWork.ArtistRepository.GetSingleWithInclude(a => a.Id == artistId, "Genres");
            if (foundArtist != null)
            {
                IList<int> genreIds = foundArtist.Genres.Select(a => a.Id).ToList();
                var genres = this._unitOfWork.GenreRepository.GetWithInclude(a => genreIds.Contains(a.Id), "Artists").ToList();
                var returnArtists = genres.SelectMany(a => a.Artists).Distinct<ms_Artist>().ToList();
                if (returnArtists.Any())
                {
                    Mapper.CreateMap<ms_Artist, ArtistEntity>();
                    var artistEntities = Mapper.Map<List<ms_Artist>, List<ArtistEntity>>(returnArtists);
                    return artistEntities;
                }
            }

            return null;
        }

        #endregion
    }
}
