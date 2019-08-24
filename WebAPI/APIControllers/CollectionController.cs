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
    public class CollectionController : ApiController
    {
        private const int NUMBER_OF_ALBUM_ON_HOMEPAGE = 20;
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private const string COLLECTION_IMAGE_PATH = @"~/Data/Collection_Images/";
        private readonly ICollectionServices _collectionServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize collection service instance
        /// </summary>
        public CollectionController(ICollectionServices collectionService)
        {
            _collectionServices = collectionService;
        }

        #endregion

        #region actions

        // GET api/collection/5
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("playlist.json")]
        public HttpResponseMessage GetCollectionById(int playlistid)
        {
            var collection = _collectionServices.GetCollectionById(playlistid);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CollectionEntity, CollectionSummary>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(COLLECTION_IMAGE_PATH + albs.Thumbnail).Replace("https", "http") : String.Empty))
                .ForMember(ae => ae.Url, map => map.MapFrom(albs =>  String.Empty)));
            var mapper = config.CreateMapper();
            var result = mapper.Map<CollectionEntity, CollectionSummary> (collection);
            if (result != null)
                return Request.CreateResponse(HttpStatusCode.OK, result);

            throw new Exception("No collection found for this id");
        }

        [Route("playlist-songs.json")]
        public HttpResponseMessage GetSongsOfCollection(int playlistid)
        {
            var songs = _collectionServices.GetSongsOfCollection(playlistid);
            IList<SummarySongModel> listSongs = new List<SummarySongModel>();
            if (songs != null && songs.Any())
            {
                var songEntities = songs as List<SongEntity> ?? songs.ToList();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SummarySongModel>()
                .ForMember(s => s.MediaUrl, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.MediaUrl) ? Url.Content(SONG_BASE_PATH + ss.MediaUrl) : String.Empty))
                .ForMember(s => s.AlbumThumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + ss.AlbumThumbnail).Replace("https", "http") : String.Empty))
                .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail).Replace("https", "http") : String.Empty)));
                var mapper = config.CreateMapper();
                listSongs = mapper.Map<List<SongEntity>, List<SummarySongModel>>(songEntities);
            }

            GetSongsOfCollectionResponse result = new GetSongsOfCollectionResponse()
            {
                Songs = listSongs
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("playlist.json")]
        [Route("api/Collection/GetFeaturedCollections")]
        public HttpResponseMessage GetFeaturedCollections()
        {
            var featuredCollections = _collectionServices.GetFeaturedCollections();
            IList<CollectionSummary> listCollectionModels = new List<CollectionSummary>();
            if (featuredCollections != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CollectionEntity, CollectionSummary>()
                 .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(COLLECTION_IMAGE_PATH + albs.Thumbnail).Replace("https", "http") : String.Empty)));
                var mapper = config.CreateMapper();
                listCollectionModels = mapper.Map<IList<CollectionEntity>, IList<CollectionSummary>>(featuredCollections.ToList());
            }

            GetFeaturedCollections result = new GetFeaturedCollections()
            {
                Collections = listCollectionModels
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        #endregion
    }
}
