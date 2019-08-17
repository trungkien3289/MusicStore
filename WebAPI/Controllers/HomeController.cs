using AutoMapper;
using Helper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using MusicStore.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private const int NUMBER_OF_ALBUM_ON_HOMEPAGE = 20;
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string SONG_PATH = @"~/Data/Album_Songs/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private readonly IAlbumServices _albumServices;
        private readonly ISongServices _songServices;
        private readonly IGenreServices _genreServices;
        private readonly IArtistServices _artistServices;

        #region Public Constructors

        public HomeController(IAlbumServices albumServices, ISongServices songServices, IGenreServices genreServices, IArtistServices artistServices)
        {
            _albumServices = albumServices;
            _songServices = songServices;
            _genreServices = genreServices;
            _artistServices = artistServices;
        }

        #endregion

        public ActionResult Index()
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            HomeViewModel viewModel = new HomeViewModel();
            // Get top albums
            IList<AlbumEntity> albums = _albumServices.GetTopAlbums(NUMBER_OF_ALBUM_ON_HOMEPAGE).ToList();
            IList<AlbumSummary> listAlbumModels = new List<AlbumSummary>();
            var albumMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<AlbumEntity, AlbumSummary>()
                   .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)))
                   .ForMember(ae => ae.ReleaseDate, map => map.MapFrom(albs => albs.ReleaseDate != null ? ((DateTime)albs.ReleaseDate).ToString("yyyy") : String.Empty)));
            var albumMapper = albumMapperConfig.CreateMapper();
            if (albums != null)
            {
                listAlbumModels = albumMapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(albums);
            }
            viewModel.TopAlbums = listAlbumModels;

            // Get featured songs
            var listSongs = _songServices.GetFeaturedSongs().ToList();
            var songMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SongEntity>()
                   .ForMember(ae => ae.MediaUrl, map => map.MapFrom(albs => DomainName + Url.Content(SONG_PATH + albs.MediaUrl)))
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(s => DomainName + Url.Content(ARTIST_IMAGE_PATH + s.Thumbnail))));
            var songMapper = songMapperConfig.CreateMapper();
            viewModel.TopSongs = songMapper.Map<IList<SongEntity>, IList<SongEntity>>(listSongs);
            // Get feature albums
            IList<AlbumSummary> listFeaturedAlbumModels = new List<AlbumSummary>();
            IList<AlbumEntity> featuredAlbums = _albumServices.GetFeaturedAlbums().ToList();
            if (featuredAlbums != null)
            {
                listFeaturedAlbumModels = albumMapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(featuredAlbums);
            }
            viewModel.FeaturedAlbums = listFeaturedAlbumModels;

            return View(viewModel);
        }

        private IList<SummarySongModel> GetFavoriteSong()
        {
            IList<SummarySongModel> songs = new List<SummarySongModel>();
            songs.Add(new SummarySongModel()
            {
                Title = "All This Is - Joe L.'s Studio",
                MediaUrl = "/Data/Audio/AC_ATKMTake_2.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "The Forsaken - Broadwing Studio (Final Mix)",
                MediaUrl = "/Data/Audio/AC_M.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All The King's Men - Broadwing Studio (Final Mix)",
                MediaUrl = "/Data/Audio/AC_TSOWAfucked_up.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "The Forsaken - Broadwing Studio (First Mix)",
                MediaUrl = "/Data/Audio/BS_ATKM.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All The King's Men - Broadwing Studio (First Mix)",
                MediaUrl = "/Data/Audio/BS_TF.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All This Is - Alternate Cuts",
                MediaUrl = "/Data/Audio/BSFM_ATKM.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All The King's Men (Take 1) - Alternate Cuts",
                MediaUrl = "/Data/Audio/BSFM_TF.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "All The King's Men (Take 2) - Alternate Cuts",
                MediaUrl = "/Data/Audio/JLS_ATI.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "Magus - Alternate Cuts",
                MediaUrl = "/Data/Audio/PNY04-05_M.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "The State Of Wearing Address (fucked up) - Alternate Cuts",
                MediaUrl = "/Data/Audio/PNY04-05_M.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = ">Magus - Popeye's (New Years '04 - '05)",
                MediaUrl = "/Data/Audio/PNY04-05_T.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "On The Waterfront - Popeye's (New Years '04 - '05)",
                MediaUrl = "/Data/Audio/PNY04-05_TSOWA.mp3",
                Thumbnail = ""
            });
            songs.Add(new SummarySongModel()
            {
                Title = "Trance - Popeye's (New Years '04 - '05)",
                MediaUrl = "/Data/Audio/SSB06_06_03_I.mp3",
                Thumbnail = ""
            });

            return songs;
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult GenresMenu()
        {
            var genres = _genreServices.GetAllGenres().ToList();
            return PartialView(genres);
        }

        public ActionResult Search(string q, int page = 1, int numberItemsPerPage = Constants.NumberItemsPerPage)
        {
            var songs = _songServices.SearchByName(q, page, numberItemsPerPage);
            IList<SongEntity> listSongModels = new List<SongEntity>();
            if (songs != null && songs.Any())
            {
                var songMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SongEntity>()
                 .ForMember(s => s.MediaUrl, map => map.MapFrom(ss => !string.IsNullOrEmpty(ss.MediaUrl) ? Url.Content(SONG_PATH + ss.MediaUrl) : ""))
                    .ForMember(s => s.AlbumThumbnail, map => map.MapFrom(ss => !string.IsNullOrEmpty(ss.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + ss.AlbumThumbnail) : "")));
                var songMapper = songMapperConfig.CreateMapper();
                listSongModels = songMapper.Map<IList<SongEntity>, IList<SongEntity>>(songs.ToList());

                foreach (var item in listSongModels)
                {
                    item.Thumbnail = !string.IsNullOrEmpty(item.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + item.Thumbnail) : String.Empty;
                }
            }

            var albums = _albumServices.SearchByName(q, page, numberItemsPerPage);
            IList<AlbumEntity> listAlbumModels = new List<AlbumEntity>();
            if (albums != null && albums.Any())
            {
                var albumMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<AlbumEntity, AlbumEntity>()
                  .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail) : "")));
                var albumMapper = albumMapperConfig.CreateMapper();
                listAlbumModels = albumMapper.Map<IList<AlbumEntity>, IList<AlbumEntity>>(albums.ToList());
            }

            var artists = _artistServices.SearchByName(q, page, numberItemsPerPage);
            IList<ArtistEntity> listArtistModels = new List<ArtistEntity>();
            var artistMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
                 .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail) : "")));
            var artistMapper = artistMapperConfig.CreateMapper();
            if (artists != null && artists.Any())
            {
                listArtistModels = artistMapper.Map<IList<ArtistEntity>, IList<ArtistEntity>>(artists.ToList());
            }

            var featuredArtists = _artistServices.GetFeaturedArtists().ToList();
            var topArtists = artistMapper.Map<List<ArtistEntity>, List<ArtistEntity>>(featuredArtists);

            ViewBag.queryString = q;

            WebSearchResult result = new WebSearchResult()
            {
                Songs = listSongModels,
                Albums = listAlbumModels,
                Artists = listArtistModels,
                TopArtists = topArtists
            };

            return View(result);
        }

        public ActionResult SearchSong(string q, int page = 1, int numberItemsPerPage = Constants.NumberItemsPerPage)
        {
            var songs = _songServices.SearchByName(q, page, numberItemsPerPage);
            IList<SongEntity> listSongModels = new List<SongEntity>();
            if (songs != null && songs.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SongEntity>()
                 .ForMember(s => s.MediaUrl, map => map.MapFrom(ss => !string.IsNullOrEmpty(ss.MediaUrl) ? Url.Content(SONG_PATH + ss.MediaUrl) : ""))
                .ForMember(s => s.AlbumThumbnail, map => map.MapFrom(ss => !string.IsNullOrEmpty(ss.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + ss.AlbumThumbnail) : "")));
                var mapper = config.CreateMapper();
                listSongModels = mapper.Map<IList<SongEntity>, IList<SongEntity>>(songs.ToList());
                foreach (var item in listSongModels)
                {
                    item.Thumbnail = !string.IsNullOrEmpty(item.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + item.Thumbnail) : String.Empty;
                }
            }

            return PartialView("_ListSongSummaryItem", listSongModels);
        }

        public ActionResult SearchAlbum(string q, int page = 1, int numberItemsPerPage = Constants.NumberItemsPerPage)
        {
            var albums = _albumServices.SearchByName(q, page, numberItemsPerPage);
            IList<AlbumEntity> listAlbumModels = new List<AlbumEntity>();
            if (albums != null && albums.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<AlbumEntity, AlbumEntity>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail) : "")));
                var mapper = config.CreateMapper();
                listAlbumModels = mapper.Map<IList<AlbumEntity>, IList<AlbumEntity>>(albums.ToList());
            }

            return PartialView("_ListAlbumSearchSummaryItem", listAlbumModels);
        }

        public ActionResult SearchArtist(string q, int page = 1, int numberItemsPerPage = Constants.NumberItemsPerPage)
        {
            var artists = _artistServices.SearchByName(q, page, numberItemsPerPage);
            IList<ArtistEntity> listArtistModels = new List<ArtistEntity>();
            if (artists != null && artists.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail) : "")));
                var mapper = config.CreateMapper();
                listArtistModels = mapper.Map<IList<ArtistEntity>, IList<ArtistEntity>>(artists.ToList());
            }

            return PartialView("_ListArtistSummaryItem", listArtistModels);
        }
    }
}
