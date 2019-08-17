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
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
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
        [Route("song-detail.json")]
        [Route("api/Song/Get")]
        public HttpResponseMessage Get(int id)
        {
            var song = _songServices.GetSongById(id);
            if (song != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, ShortSummarySongModel>()
                 .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail).Replace("https", "http") : String.Empty))
                  .ForMember(ae => ae.MediaUrl, map => map.MapFrom(s => !String.IsNullOrEmpty(s.MediaUrl) ? Url.Content(SONG_BASE_PATH + s.MediaUrl) : String.Empty)));
                var mapper = config.CreateMapper();
                var foundSong = mapper.Map<SongEntity, ShortSummarySongModel>(song);
                GetSongDetailsResponse response = new GetSongDetailsResponse();
                response.Songs.Add(foundSong);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            throw new Exception("No song found for this id");
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No album found for this id");
        }

        public HttpResponseMessage GetAlbumAfterGenre(string categoryName)
        {
            IList<SongEntity> songs = _songServices.GetSongsByCategory(categoryName).ToList();
            IList<SummarySongModel> listSongModels = new List<SummarySongModel>();
            if (songs != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, ShortSummarySongModel>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail).Replace("https", "http") : String.Empty))
                 .ForMember(ae => ae.MediaUrl, map => map.MapFrom(s => !String.IsNullOrEmpty(s.MediaUrl) ? Url.Content(SONG_BASE_PATH + s.MediaUrl) : String.Empty)));
                var mapper = config.CreateMapper();
                listSongModels = mapper.Map<IList<SongEntity>, IList<SummarySongModel>>(songs);
            }
            return Request.CreateResponse(HttpStatusCode.OK, listSongModels);
        }

        [Route("~/top.json")]
        [Route("api/Song/GetFeaturedSongs")]
        public HttpResponseMessage GetFeaturedSongs()
        {
            var songs = _songServices.GetFeaturedSongs();
            IList<SummarySongModel> listSongModels = new List<SummarySongModel>();
            if (songs != null && songs.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SummarySongModel>().ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + albs.AlbumThumbnail).Replace("https", "http") : String.Empty))
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail).Replace("https", "http") : String.Empty))
                    .ForMember(ae => ae.MediaUrl, map => map.MapFrom(s => !String.IsNullOrEmpty(s.MediaUrl) ? Url.Content(SONG_BASE_PATH + s.MediaUrl) : String.Empty)));
                var mapper = config.CreateMapper();
                listSongModels = mapper.Map<IList<SongEntity>, IList<SummarySongModel>>(songs.ToList());
            }

            GetFeaturedSongs result = new GetFeaturedSongs()
            {
                Songs = listSongModels
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage GetTopSongs(int top)
        {
            var songs = _songServices.GetTopSongs(top);
            IList<SummarySongModel> listSongModels = new List<SummarySongModel>();
            if (songs != null && songs.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SummarySongModel>().ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + albs.AlbumThumbnail).Replace("https", "http") : String.Empty))
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + albs.Thumbnail).Replace("https", "http") : String.Empty))
                    .ForMember(ae => ae.MediaUrl, map => map.MapFrom(s => !String.IsNullOrEmpty(s.MediaUrl) ? Url.Content(SONG_BASE_PATH + s.MediaUrl) : String.Empty)));
                var mapper = config.CreateMapper();
                listSongModels = mapper.Map<IList<SongEntity>, IList<SummarySongModel>>(songs.ToList());
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
