using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class WebAlbumController : Controller
    {
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private readonly IAlbumServices _albumServices;

        public WebAlbumController(IAlbumServices albumService)
        {
            _albumServices = albumService;
        }
        // GET: Album
        public ActionResult Index(int id)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            AlbumViewModel albumViewModel = new AlbumViewModel();
            var album = _albumServices.GetAlbumById(id);
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

                albumViewModel.Songs = listSongsOfAlbum;

                return View(albumViewModel);
            }

            return RedirectToAction("Error", "Home");
            
        }
    }
}