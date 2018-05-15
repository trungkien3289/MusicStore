using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    #region variables
    #endregion

    public class ArtistController : ApiController
    {
        private const int NUMBER_OF_ALBUM_ON_HOMEPAGE = 20;
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private readonly IArtistServices _artistServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize artist service instance
        /// </summary>
        public ArtistController(IArtistServices artistService)
        {
            _artistServices = artistService;
        }

        #endregion

        #region actions
        // GET api/artist
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get()
        {
            var artists = _artistServices.GetAllArtists();
            if (artists != null)
            {
                var artistEntities = artists as List<ArtistEntity> ?? artists.ToList();
                if (artistEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, artistEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artists not found");
        }

        // GET api/artist/5
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get(int id)
        {
            var artist = _artistServices.GetArtistById(id);
            if (artist != null)
                return Request.CreateResponse(HttpStatusCode.OK, artist);

            throw new Exception("No artist found for this id");
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No artist found for this id");
        }

        #endregion
    }
}
