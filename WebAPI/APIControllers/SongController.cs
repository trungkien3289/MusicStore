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
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
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

        #region actions
        // GET api/song/5
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get(int id)
        {
            var song = _songServices.GetSongById(id);
            song.MediaUrl = Url.Content(SONG_BASE_PATH + song.MediaUrl);
            if (song != null)
                return Request.CreateResponse(HttpStatusCode.OK, song);

            throw new Exception("No song found for this id");
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No album found for this id");
        }

        public HttpResponseMessage GetAlbumAfterGenre(string categoryName)
        {
            IList<SongEntity> songs = _songServices.GetSongsByCategory(categoryName).ToList();
            IList<SummarySongModel> listSongModels = new List<SummarySongModel>();
            if (songs != null)
            {
                Mapper.CreateMap<SongEntity, SummarySongModel>()
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)))
                    .ForMember(ae => ae.MediaUrl, map => map.MapFrom(s => Url.Content(SONG_BASE_PATH + s.MediaUrl)));
                listSongModels = Mapper.Map<IList<SongEntity>, IList<SummarySongModel>>(songs);
            }
            return Request.CreateResponse(HttpStatusCode.OK, listSongModels);
        }

        public HttpResponseMessage GetFeaturedSongs()
        {
            var songs = _songServices.GetFeaturedSongs().ToList();
            IList<SummarySongModel> listSongModels = new List<SummarySongModel>();
            if (songs.Any())
            {
                Mapper.CreateMap<SongEntity, SummarySongModel>()
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)))
                    .ForMember(ae => ae.MediaUrl, map => map.MapFrom(s => Url.Content(SONG_BASE_PATH + s.MediaUrl)));
                listSongModels = Mapper.Map<IList<SongEntity>, IList<SummarySongModel>>(songs);
            }
            return Request.CreateResponse(HttpStatusCode.OK, listSongModels);
        }

        public HttpResponseMessage GetSongLyric(int id)
        {
            var song = _songServices.GetSongById(id);
            if(song != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, song.Lyrics);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Song not found.");
            }
        }

        #endregion
    }
}
