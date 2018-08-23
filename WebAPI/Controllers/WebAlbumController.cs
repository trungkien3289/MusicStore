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
    [Route("Album")]
    public class WebAlbumController : Controller
    {
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string SONG_PATH = @"~/Data/Album_Songs/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private readonly IAlbumServices _albumServices;
        private readonly IArtistServices _artistServices;
        private const int PAGE_SIZE = 20;

        public WebAlbumController(IAlbumServices albumServices, IArtistServices artistServices)
        {
            _albumServices = albumServices;
            _artistServices = artistServices;
        }
        // GET: Album
        public ActionResult AlbumDetails(int id)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            AlbumDetailsViewModel albumViewModel = new AlbumDetailsViewModel();
            var album = _albumServices.GetAlbumWithArtists(id);
            if (album !=null)
            {
                IList<SongEntity> listSongsOfAlbum = _albumServices.GetSongsOfAlbum(id).ToList();
                var listAlbumsOfArtists = _albumServices.GetAlbumsHasSameArtists(id);
                var listAlbumsHasSameGenres = _albumServices.GetAlbumsHasSameGenres(id);

                Mapper.CreateMap<AlbumEntity, AlbumSummary>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)))
                    .ForMember(ae => ae.ReleaseDate, map => map.MapFrom(albs => albs.ReleaseDate != null ? String.Format("{0:YYYY-MM}", albs.ReleaseDate) : String.Empty));
                if (listAlbumsOfArtists != null)
                {
                    albumViewModel.AlbumsSameArtists = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(listAlbumsOfArtists.ToList());
                }

                if (listAlbumsHasSameGenres != null)
                {
                    albumViewModel.AlbumsSameGenres = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(listAlbumsHasSameGenres.ToList());
                }

                Mapper.CreateMap<SongEntity, SongEntity>().ForMember(ae => ae.MediaUrl, map => map.MapFrom(albs => DomainName + Url.Content(SONG_PATH + albs.MediaUrl)));
                albumViewModel.Songs = Mapper.Map<IList<SongEntity>, IList<SongEntity>>(listSongsOfAlbum);

                Mapper.CreateMap<AlbumEntity, AlbumEntity>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)));
                albumViewModel.AlbumDetail = Mapper.Map<AlbumEntity, AlbumEntity>(album);

                return View(albumViewModel);
            }

            return RedirectToAction("Error", "Home");
        }
        public ActionResult Index(string character = "a", int page = 1)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            AlbumViewModel returnModel = new AlbumViewModel();
            var albums = _albumServices.GetAlbumsAfterBeginCharacter(character, page, PAGE_SIZE).ToList();
            Mapper.CreateMap<AlbumEntity, AlbumSummary>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)))
                .ForMember(ae => ae.ReleaseDate, map => map.MapFrom(albs => albs.ReleaseDate != null ? String.Format("{0:yyyy}", albs.ReleaseDate) : String.Empty));
            if (albums != null)
            {
                returnModel.Albums = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(albums);
            }

            var featuredArtists = _artistServices.GetFeaturedArtists().ToList();
            Mapper.CreateMap<ArtistEntity, ArtistEntity>().ForMember(s => s.Thumbnail, map => map.MapFrom(ss => Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail)));
            returnModel.TopArtists = Mapper.Map<List<ArtistEntity>, List<ArtistEntity>>(featuredArtists);

            ViewBag.SelectedCharacter = character;

            return View(returnModel);
        }

        [HttpPost]
        public ActionResult GetPagingAlbumByCharacter(string character = "a", int page = 1)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            var albums = _albumServices.GetAlbumsAfterBeginCharacter(character, page, PAGE_SIZE);
            Mapper.CreateMap<AlbumEntity, AlbumSummary>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)))
                   .ForMember(ae => ae.ReleaseDate, map => map.MapFrom(albs => albs.ReleaseDate != null ? String.Format("{0:YYYY-MM}", albs.ReleaseDate) : String.Empty));
            IList<AlbumSummary> result = new List<AlbumSummary>();
            if (albums != null)
            {
                result = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(albums.ToList());
            }

            return PartialView("_ListAlbumSummaryItem", result);
        }
    }
}