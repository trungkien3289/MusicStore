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
    public class WebGenreController : Controller
    {
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string SONG_PATH = @"~/Data/Album_Songs/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
        private readonly IAlbumServices _albumServices;
        private readonly IArtistServices _artistServices;
        private readonly IGenreServices _genreServices;
        private const int PAGE_SIZE = 20;

        public WebGenreController(IAlbumServices albumServices, IArtistServices artistServices, IGenreServices genreServices)
        {
            _albumServices = albumServices;
            _artistServices = artistServices;
            _genreServices = genreServices;
        }

        // GET: WebGenre
        public ActionResult Index(int id, int page = 1)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            GenreViewModel returnModel = new GenreViewModel();
            var genre = _genreServices.GetGenreDetails(id);
            returnModel.Id = id;
            returnModel.Name = genre.Name;
            var artists = _genreServices.GetArtistsOfGenre(id, page, PAGE_SIZE);
            Mapper.CreateMap<ArtistEntity, ArtistEntity>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail)));
            if (artists != null)
            {
                returnModel.Artists = Mapper.Map<IList<ArtistEntity>, IList<ArtistEntity>>(artists.ToList());
            }

            var featuredSongs = _genreServices.GetSongsOfGenre(id).Take(10).ToList();

            if (featuredSongs.Any())
            {
                Mapper.CreateMap<SongEntity, SongEntity>()
                     .ForMember(a => a.AlbumThumbnail, map => map.MapFrom(al => !String.IsNullOrEmpty(al.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + al.AlbumThumbnail) : String.Empty))
                    .ForMember(a => a.Thumbnail, map => map.MapFrom(al => !String.IsNullOrEmpty(al.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + al.Thumbnail) : String.Empty))
                    .ForMember(a => a.MediaUrl, map => map.MapFrom(al => !String.IsNullOrEmpty(al.MediaUrl) ? Url.Content(SONG_BASE_PATH + al.MediaUrl) : String.Empty));
                returnModel.Songs = Mapper.Map<List<SongEntity>, List<SongEntity>>(featuredSongs);
            }

            return View(returnModel);
        }

        [HttpPost]
        public ActionResult GetPagingArtist(int id, int page = 1)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            var artists = _genreServices.GetArtistsOfGenre(id, page, PAGE_SIZE);
            Mapper.CreateMap<ArtistEntity, ArtistEntity>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail)));
            IList<ArtistEntity> result = new List<ArtistEntity>();
            if (artists != null)
            {
                result = Mapper.Map<IList<ArtistEntity>, IList<ArtistEntity>>(artists.ToList());
            }

            return PartialView("_ListArtistSummaryItem", result);
        }
    }
}