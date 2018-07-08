using AutoMapper;
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
        private readonly IAlbumServices _albumServices;
        private readonly ISongServices _songServices;

        #region Public Constructors

        public HomeController(IAlbumServices albumServices, ISongServices songServices)
        {
            _albumServices = albumServices;
            _songServices = songServices;
        }

        #endregion

        public ActionResult Index()
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            HomeViewModel viewModel = new HomeViewModel();
            // Get top albums
            IList<AlbumEntity> albums = _albumServices.GetTopAlbums(NUMBER_OF_ALBUM_ON_HOMEPAGE).ToList();
            IList<AlbumSummary> listAlbumModels = new List<AlbumSummary>();
            if (albums != null)
            {
                Mapper.CreateMap<AlbumEntity, AlbumSummary>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ALBUM_IMAGE_PATH+albs.Thumbnail)));
                listAlbumModels = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(albums);
            }
            viewModel.TopAlbums = listAlbumModels;
            // Get featured songs
            viewModel.TopSongs = _songServices.GetFeaturedSongs().ToList(); ;
            // Get feature albums
            IList<AlbumSummary> listFeaturedAlbumModels = new List<AlbumSummary>();
            IList<AlbumEntity> featuredAlbums = _albumServices.GetFeaturedAlbums().ToList();
            if (featuredAlbums != null)
            {
                Mapper.CreateMap<AlbumEntity, AlbumSummary>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)))
                    .ForMember(ae => ae.ReleaseDate, map => map.MapFrom(albs => albs.ReleaseDate!=null? String.Format("{0:YYYY-MM}", albs.ReleaseDate):String.Empty));
                listFeaturedAlbumModels = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(featuredAlbums);
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
    }
}
