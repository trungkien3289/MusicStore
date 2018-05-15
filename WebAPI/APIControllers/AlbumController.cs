using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IServices;
using MusicStore.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Filters;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    //[ApiAuthenticationFilter]
    //[AuthorizationRequired]
    public class AlbumController : ApiController
    {
        private const int NUMBER_OF_ALBUM_ON_HOMEPAGE = 20;
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private readonly IAlbumServices _albumServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize album service instance
        /// </summary>
        public AlbumController(IAlbumServices albumService)
        {
            _albumServices = albumService;
        }

        #endregion

        // GET api/album
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get()
        {
            var albums = _albumServices.GetAllAlbums();
            if (albums != null)
            {
                var albumEntities = albums as List<AlbumEntity> ?? albums.ToList();
                if (albumEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, albumEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Albums not found");
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetTopAlbums()
        {
            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IList<AlbumEntity> albums = _albumServices.GetTopAlbums(NUMBER_OF_ALBUM_ON_HOMEPAGE).ToList();
            IList<AlbumSummary> listAlbumModels = new List<AlbumSummary>();
            if (albums != null)
            {
                Mapper.CreateMap<AlbumEntity, AlbumSummary>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => baseUrl + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)));
                listAlbumModels = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(albums);
            }
            return Request.CreateResponse(HttpStatusCode.OK, listAlbumModels);
        }

        // GET api/album/5
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get(int id)
        {
            var album = _albumServices.GetAlbumById(id);
            if (album != null)
                return Request.CreateResponse(HttpStatusCode.OK, album);

            throw new Exception("No album found for this id");
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No album found for this id");
        }

        [Route("g/{categoryName}")]
        public HttpResponseMessage GetAlbumAfterGenre(string categoryName)
        {
            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IList<AlbumEntity> albums = _albumServices.GetAlbumsByCategory(categoryName).ToList();
            IList<AlbumSummary> listAlbumModels = new List<AlbumSummary>();
            if (albums != null)
            {
                Mapper.CreateMap<AlbumEntity, AlbumSummary>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => baseUrl + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)));
                listAlbumModels = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(albums);
            }
            return Request.CreateResponse(HttpStatusCode.OK, listAlbumModels);
        }

        // POST api/album
        public int Post([FromBody]AlbumEntity albumEntity)
        {
            return _albumServices.CreateAlbum(albumEntity);
        }

        // PUT api/album/5
        public bool Put(int id, [FromBody]AlbumEntity albumEntity)
        {
            if (id > 0)
            {
                return _albumServices.UpdateAlbum(id, albumEntity);
            }
            return false;
        }

        // DELETE api/album/5
        public bool Delete(int id)
        {
            if (id > 0)
                return _albumServices.DeleteAlbum(id);
            return false;
        }
    }
}
