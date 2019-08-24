using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class WebSongController : Controller
    {

        private const int NUMBER_OF_ALBUM_ON_HOMEPAGE = 20;
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
        private readonly IAlbumServices _albumServices;
        private readonly IArtistServices _artistServices;
        private readonly ISongServices _songServices;
        private const int PAGE_SIZE = 20;

        public WebSongController(IAlbumServices albumServices, IArtistServices artistServices, ISongServices songServices)
        {
            _albumServices = albumServices;
            _artistServices = artistServices;
            _songServices = songServices;
        }

        // GET: WebSong
        public ActionResult Index(int id)
        {
            SongDetailsViewModel viewModel = new SongDetailsViewModel();
            var song = _songServices.GetSongById(id);
            if (song != null)
            {
                var songMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, ShortSummarySongModel>()
                 .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail) : String.Empty))
                  .ForMember(ae => ae.MediaUrl, map => map.MapFrom(s => !String.IsNullOrEmpty(s.MediaUrl) ? Url.Content(SONG_BASE_PATH + s.MediaUrl) : String.Empty)));
                var songMapper = songMapperConfig.CreateMapper();
                var foundSong = songMapper.Map<SongEntity, ShortSummarySongModel>(song);
                viewModel.SongDetails = foundSong;

                // Get top Artists
                var featuredArtists = _artistServices.GetFeaturedArtists().ToList();
                var artistMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
                .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail))));
                var artistMapper = artistMapperConfig.CreateMapper();
                viewModel.TopArtists = artistMapper.Map<List<ArtistEntity>, List<ArtistEntity>>(featuredArtists);
                return View(viewModel);
            }

            return RedirectToAction("Error", "Home");
        }
    }
}