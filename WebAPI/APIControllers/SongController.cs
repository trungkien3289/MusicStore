using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.APIControllers
{
    public class SongController : ApiController
    {
        private const int NUMBER_OF_ALBUM_ON_HOMEPAGE = 20;
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private readonly ISongServices _songServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize song service instance
        /// </summary>
        public SongController(ISongServices songServices)
        {
            _songServices = songServices;
        }

        #endregion

        // GET api/song/5
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get(int id)
        {
            var song = _songServices.GetSongById(id);
            if (song != null)
                return Request.CreateResponse(HttpStatusCode.OK, song);

            throw new Exception("No song found for this id");
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No album found for this id");
        }

        //[Route("g/{categoryName}")]
        public HttpResponseMessage GetAlbumAfterGenre(string categoryName)
        {
            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IList<SongEntity> songs = _songServices.GetSongsByCategory(categoryName).ToList();
            IList<SummarySongModel> listSongModels = new List<SummarySongModel>();
            if (songs != null)
            {
                Mapper.CreateMap<SongEntity, SummarySongModel>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => baseUrl + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)));
                listSongModels = Mapper.Map<IList<SongEntity>, IList<SummarySongModel>>(songs);
            }
            return Request.CreateResponse(HttpStatusCode.OK, listSongModels);
        }
    }
}
