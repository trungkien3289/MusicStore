using AutoMapper;
using Helper;
using MusicStore.BussinessEntity;
using MusicStore.BussinessEntity.Enums;
using MusicStore.Service.IService;
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
    public class HomeAPIController : ApiController
    {
        private const int NUMBER_OF_ALBUM_ON_HOMEPAGE = 20;
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private readonly ISongServices _songServices;
        private readonly IAlbumServices _albumServices;
        private readonly IArtistServices _artistServices;
        private readonly IApplicationServices _applicationServices;
        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize song service instance
        /// </summary>
        public HomeAPIController(ISongServices songServices, IAlbumServices albumServices, IArtistServices artistServices, IApplicationServices applicationServices)
        {
            _songServices = songServices;
            _albumServices = albumServices;
            _artistServices = artistServices;
            _applicationServices = applicationServices;
        }

        #endregion

        #region actions
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("full-search.json")]
        public HttpResponseMessage Search(string q, int page = 1, int numberItemsPerPage = Constants.NumberItemsPerPage)
        {
            var songs = _songServices.SearchByName(q, page, numberItemsPerPage);
            IList<SongEntity> listSongModels = new List<SongEntity>();
            if (songs != null && songs.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SongEntity, SongEntity>()
                  .ForMember(s => s.MediaUrl, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.MediaUrl) ? Url.Content(SONG_BASE_PATH + ss.MediaUrl) : String.Empty))
                    .ForMember(s => s.AlbumThumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + ss.AlbumThumbnail).Replace("https", "http") : String.Empty))
                    .ForMember(s => s.Thumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail).Replace("https", "http") : String.Empty)));
                var mapper = config.CreateMapper();

                listSongModels = mapper.Map<IList<SongEntity>, IList<SongEntity>>(songs.ToList());
            }

            var albums = _albumServices.SearchByName(q, page, numberItemsPerPage);
            IList<AlbumEntity> listAlbumModels = new List<AlbumEntity>();
            if (albums != null && albums.Any())
            {
                var albumMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<AlbumEntity, AlbumEntity>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail).Replace("https", "http") : "")));
                var albumMapper = albumMapperConfig.CreateMapper();
                listAlbumModels = albumMapper.Map<IList<AlbumEntity>, IList<AlbumEntity>>(albums.ToList());
            }

            var artists = _artistServices.SearchByName(q, page, numberItemsPerPage);
            IList<ArtistEntity> listArtistModels = new List<ArtistEntity>();
            if (artists != null && artists.Any())
            {
                var artistMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<ArtistEntity, ArtistEntity>()
               .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail).Replace("https", "http") : "")));
                var artistMapper = artistMapperConfig.CreateMapper();
                listArtistModels = artistMapper.Map<IList<ArtistEntity>, IList<ArtistEntity>>(artists.ToList());
            }

            SearchResult result = new SearchResult()
            {
                Songs = listSongModels,
                Albums = listAlbumModels,
                Artists = listArtistModels
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("wdg-ads-list.json")]
        public HttpResponseMessage GetListApplications()
        {
            var applicationEntitys = _applicationServices.GetAllApplications();
            IList<ApplicationEntity> applications = new List<ApplicationEntity>();
            if (applicationEntitys != null && applicationEntitys.Any())
            {
                applications = applicationEntitys.ToList();
            }

            GetListApplicationsResponse result = new GetListApplicationsResponse()
            {
                Apllications = applications
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
        #endregion
    }
}
