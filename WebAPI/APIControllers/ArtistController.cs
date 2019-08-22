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

namespace WebAPI.Controllers
{
    public class ArtistController : ApiController
    {
        #region variables
        private const int NUMBER_OF_ALBUM_ON_HOMEPAGE = 20;
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
        private readonly IArtistServices _artistServices;

        #endregion

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
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
                    .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail).Replace("https", "http"))));
                    var mapper = config.CreateMapper();
                    var artistLists = mapper.Map<List<ArtistEntity>, List<ArtistEntity>>(artistEntities);
                    return Request.CreateResponse(HttpStatusCode.OK, artistLists);
                }
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

        [Route("artists.json")]
        [Route("api/Artist/GetFeaturedArtists")]
        public HttpResponseMessage GetFeaturedArtists()
        {
            var artists = _artistServices.GetFeaturedArtists();
            var artistLists = new List<ArtistEntity>();

            if (artists != null)
            {
                if (artists != null && artists.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
                    .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail).Replace("https", "http"))));
                    var mapper = config.CreateMapper();
                    artistLists = mapper.Map<List<ArtistEntity>, List<ArtistEntity>>(artists.ToList());
                }
            }

            GetFeaturedArtists result = new GetFeaturedArtists()
            {
                Artists = artistLists
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);

            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no artist marked as featured.");
        }

        public HttpResponseMessage GetSongsOfArtist(int id)
        {
            var songs = _artistServices.GetSongsOfArtist(id).ToList();
            if (songs != null)
            {
                if (songs.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SummarySongModel>()
                    .ForMember(s => s.MediaUrl, map => map.MapFrom(ss => Url.Content(SONG_BASE_PATH + ss.MediaUrl))));
                    var mapper = config.CreateMapper();
                    var songModels = mapper.Map<List<SongEntity>, List<SummarySongModel>>(songs);
                    return Request.CreateResponse(HttpStatusCode.OK, songModels);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no song belong to artist.");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot found artist.");
        }

        // TODO: get top items from database instead load all items and get top from memory
        [Route("artist-songs.json")]
        public HttpResponseMessage GetTopSongsOfArtist(int artistid, int top = 0)
        {
            var songs = _artistServices.GetSongsOfArtist(artistid);
            IList<SummarySongModel> listSongsOfArtist = new List<SummarySongModel>();
            if (songs != null && songs.Any())
            {
                if (top != 0)
                {
                    songs = songs.Take(top).ToList();
                }
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SummarySongModel>()
                 .ForMember(s => s.MediaUrl, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.MediaUrl) ? Url.Content(SONG_BASE_PATH + ss.MediaUrl) : String.Empty))
                .ForMember(s => s.AlbumThumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + ss.AlbumThumbnail).Replace("https", "http") : String.Empty))
                .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail).Replace("https", "http") : String.Empty)));
                var mapper = config.CreateMapper();
                listSongsOfArtist = mapper.Map<List<SongEntity>, List<SummarySongModel>>(songs.ToList());
            }

            GetTopSongsOfArtistResponse result = new GetTopSongsOfArtistResponse()
            {
                Songs = listSongsOfArtist
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage GetAlbumsOfArtist(int id)
        {
            var albums = _artistServices.GetAlbumsOfArtist(id).ToList();
            if (albums != null)
            {
                if (albums.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<AlbumEntity, AlbumEntity>()
                    .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => Url.Content(ALBUM_IMAGE_PATH + ss.Thumbnail).Replace("https", "http"))));
                    var mapper = config.CreateMapper();
                    var albumModels = mapper.Map<List<AlbumEntity>, List<AlbumEntity>>(albums);
                    return Request.CreateResponse(HttpStatusCode.OK, albumModels);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no album belong to artist.");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot found artist.");
        }

        #endregion
    }
}
