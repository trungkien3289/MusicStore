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
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
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
            IList<AlbumEntity> albums = _albumServices.GetTopAlbums(NUMBER_OF_ALBUM_ON_HOMEPAGE).ToList();
            IList<AlbumSummary> listAlbumModels = new List<AlbumSummary>();
            if (albums != null)
            {
                Mapper.CreateMap<AlbumEntity, AlbumSummary>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)));
                listAlbumModels = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(albums);
            }
            return Request.CreateResponse(HttpStatusCode.OK, listAlbumModels);
        }

        // GET api/album/5
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetAlbum(int id)
        {
            var album = _albumServices.GetAlbumById(id);
            if (album != null)
                return Request.CreateResponse(HttpStatusCode.OK, album);

            throw new Exception("No album found for this id");
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No album found for this id");
        }

        // GET api/album/5
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        //[Route("playlist.json")]
        public HttpResponseMessage GetAlbumById(int playlistid)
        {
            var album = _albumServices.GetAlbumById(playlistid);
            if (album != null)
                return Request.CreateResponse(HttpStatusCode.OK, album);

            throw new Exception("No album found for this id");
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No album found for this id");
        }

        public HttpResponseMessage GetSongsOfAlbum(int id)
        {
            var songs = _albumServices.GetSongsOfAlbum(id);
            if (songs!= null)
            {
                var songEntities = songs as List<SongEntity> ?? songs.ToList();
                if (songEntities.Any())
                {
                    Mapper.CreateMap<SongEntity, SummarySongModel>().ForMember(s => s.MediaUrl, map => map.MapFrom(ss => Url.Content(SONG_BASE_PATH + ss.MediaUrl)));
                    var songModels = Mapper.Map<List<SongEntity>, List<SummarySongModel>>(songEntities);
                    return Request.CreateResponse(HttpStatusCode.OK, songModels);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no any songs in album.");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot found album.");
        }

        // TODO : review this action, which has the same funtionality with  "GetAlbumById" action
        //[Route("playlist-song.json")]
        public HttpResponseMessage GetSongsOfAlbum2(int playlistid)
        {
            var songs = _albumServices.GetSongsOfAlbum(playlistid);
            IList<SummarySongModel> listSongs = new List<SummarySongModel>();
            if (songs != null && songs.Any())
            {
                var songEntities = songs as List<SongEntity> ?? songs.ToList();
                    Mapper.CreateMap<SongEntity, SummarySongModel>()
                    .ForMember(s => s.MediaUrl, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.MediaUrl)?Url.Content(SONG_BASE_PATH + ss.MediaUrl):String.Empty))
                    .ForMember(s => s.AlbumThumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.AlbumThumbnail)?Url.Content(ALBUM_IMAGE_PATH + ss.AlbumThumbnail):String.Empty))
                    .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.Thumbnail)?Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail):String.Empty));
                listSongs = Mapper.Map<List<SongEntity>, List<SummarySongModel>>(songEntities);
            }

            GetSongsOfAlbumResponse result = new GetSongsOfAlbumResponse()
            {
                Songs = listSongs
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
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

        //[Route("playlist.json")]
        [Route("api/Album/GetFeaturedAlbums")]
        public HttpResponseMessage GetFeaturedAlbums()
        {
            var featuredAlbums = _albumServices.GetFeaturedAlbums();
            IList<AlbumSummary> listAlbumModels = new List<AlbumSummary>();
            if (featuredAlbums != null)
            {
                Mapper.CreateMap<AlbumEntity, AlbumSummary>().ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs =>!String.IsNullOrEmpty(albs.Thumbnail)? Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail):String.Empty));
                listAlbumModels = Mapper.Map<IList<AlbumEntity>, IList<AlbumSummary>>(featuredAlbums.ToList());
            }

            GetFeaturedAlbums result = new GetFeaturedAlbums()
            {
                Albums = listAlbumModels
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("albums.json")]
        public HttpResponseMessage GetAlbumsOfArtist(int artistid, int top = 0)
        {
            var albums = _albumServices.GetAlbumsOfArtist(artistid, top);
            IList<AlbumShortSummary> listAlbumModels = new List<AlbumShortSummary>();
            if (albums != null)
            {
                Mapper.CreateMap<AlbumEntity, AlbumShortSummary>()
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail)? Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail):String.Empty))
                    .ForMember(ae => ae.ReleaseDate, map => map.MapFrom(albs => albs.ReleaseDate.Value.Year.ToString()))
                    .ForMember(ae => ae.Slug, map => map.MapFrom(albs => albs.Url));
                listAlbumModels = Mapper.Map<IList<AlbumEntity>, IList<AlbumShortSummary>>(albums.ToList());
            }

            GetAlbumsOfArtistResponse result = new GetAlbumsOfArtistResponse()
            {
                Albums = listAlbumModels
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);
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
