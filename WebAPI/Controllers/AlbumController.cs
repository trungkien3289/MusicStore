using MusicStore.BussinessEntity;
using MusicStore.Service.IServices;
using MusicStore.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ApiAuthenticationFilter]
    public class AlbumController : ApiController
    {
        private readonly IAlbumServices _albumServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize album service instance
        /// </summary>
        public AlbumController(IAlbumServices albumService)
        {
            _albumServices = albumService;

        }
        //public AlbumController()
        //{
        //    _albumServices = new AlbumServices();

        //}

        #endregion

        // GET api/album
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

        // GET api/album/5
        public HttpResponseMessage Get(int id)
        {
            var album = _albumServices.GetAlbumById(id);
            if (album != null)
                return Request.CreateResponse(HttpStatusCode.OK, album);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No album found for this id");
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
