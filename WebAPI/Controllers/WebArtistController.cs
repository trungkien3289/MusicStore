using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class WebArtistController : Controller
    {
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string SONG_PATH = @"~/Data/Album_Songs/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private readonly IAlbumServices _albumServices;
        private readonly IArtistServices _artistServices;
        private const int PAGE_SIZE = 20;


        public WebArtistController(IAlbumServices albumServices, IArtistServices artistServices)
        {
            _albumServices = albumServices;
            _artistServices = artistServices;
        }
        // GET: WebArtist
        public ActionResult Index(string character = "a", int page = 1)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            ArtistViewResponse returnModel = new ArtistViewResponse();
            var artists = _artistServices.GetArtistsAfterBeginCharacter(character, page, PAGE_SIZE).ToList();
            var artistMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
             .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail)))
                .ForMember(a => a.NumberOfSongs, map => map.MapFrom(albs => albs.NumberOfSongs))
                .ForMember(a => a.NumberOfAlbums, map => map.MapFrom(albs => albs.NumberOfAlbums)));
            var artistMapper = artistMapperConfig.CreateMapper();
            if (artists != null)
            {
                returnModel.Artists = artistMapper.Map<IList<ArtistEntity>, IList<ArtistEntity>>(artists.ToList());
            }

            var featuredArtists = _artistServices.GetFeaturedArtists().ToList();
            returnModel.FeaturedArtists = Mapper.Map<List<ArtistEntity>, List<ArtistEntity>>(featuredArtists);

            var featuredAlbums = _albumServices.GetFeaturedAlbums().ToList();
            var albumMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<AlbumEntity, AlbumSummary>()
                .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => Url.Content(ALBUM_IMAGE_PATH + ss.Thumbnail))));
            var albumMapper = albumMapperConfig.CreateMapper();
            returnModel.FeaturedAlbums = albumMapper.Map<List<AlbumEntity>, List<AlbumSummary>>(featuredAlbums);

            ViewBag.SelectedCharacter = character;

            return View(returnModel);
        }

        public ActionResult Details(int id)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            ArtistDetailsViewModel artistViewModel = new ArtistDetailsViewModel();
            var artist = _artistServices.GetArtistById(id);
            if (artist != null)
            {
                IList<SongEntity> listSongsOfArtists = _artistServices.GetSongsOfArtist(id).ToList();
                var listAlbumsOfArtists = _artistServices.GetAlbumsOfArtist(id);
                var listArtistHasSameGenres = _artistServices.GetArtistsHasSameGenre(id);

                var albumMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<AlbumEntity, AlbumSummary>()
                .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => Url.Content(ALBUM_IMAGE_PATH + ss.Thumbnail))));
                var albumMapper = albumMapperConfig.CreateMapper();
                if (listAlbumsOfArtists != null)
                {
                    artistViewModel.Albums = albumMapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(listAlbumsOfArtists.ToList());
                }

                var artistMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail))));
                var artistMapper = artistMapperConfig.CreateMapper();
                if (listArtistHasSameGenres != null)
                {
                    artistViewModel.RelatedArtists = artistMapper.Map<IList<ArtistEntity>, IList<ArtistEntity>>(listArtistHasSameGenres.ToList());
                }

                var songMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SongEntity>()
                    .ForMember(ae => ae.MediaUrl, map => map.MapFrom(albs => DomainName + Url.Content(SONG_PATH + albs.MediaUrl))));
                var songMapper = songMapperConfig.CreateMapper();
                artistViewModel.Songs = songMapper.Map<IList<SongEntity>, IList<SongEntity>>(listSongsOfArtists);

                artistViewModel.Details = artistMapper.Map<ArtistEntity, ArtistEntity>(artist);

                return View(artistViewModel);
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public ActionResult GetPagingArtistByCharacter(string character = "a", int page = 1)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            var artists = _artistServices.GetArtistsAfterBeginCharacter(character, page, PAGE_SIZE);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail))));
            var mapper = config.CreateMapper();
            IList<ArtistEntity> result = new List<ArtistEntity>();
            if (artists != null)
            {
                result = mapper.Map<IList<ArtistEntity>, IList<ArtistEntity>>(artists.ToList());
            }

            return PartialView("_ListArtistSummaryItem", result);
        }
    }
}