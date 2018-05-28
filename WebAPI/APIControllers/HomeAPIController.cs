using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.BussinessEntity.Enums;
using MusicStore.Service.IService;
using MusicStore.Service.IServices;
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
        private readonly ISongServices _songServices;
        private readonly IAlbumServices _albumServices;
        private readonly IArtistServices _artistServices;
        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize song service instance
        /// </summary>
        public HomeAPIController(ISongServices songServices, IAlbumServices albumServices, IArtistServices artistServices)
        {
            _songServices = songServices;
            _albumServices = albumServices;
            _artistServices = artistServices;
        }

        #endregion

        #region actions
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Search(string query)
        {
            var songs = _songServices.SearchByName(query).ToList();
            IList<SearchResultEntity> listSongModels = new List<SearchResultEntity>();
            if (songs.Any())
            {
                Mapper.CreateMap<SongEntity, SearchResultEntity>()
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content( ALBUM_IMAGE_PATH + albs.Thumbnail) : ""))
                    .ForMember(ae => ae.Type, map => map.MapFrom(albs => ItemType.Song));
                listSongModels = Mapper.Map<IList<SongEntity>, IList<SearchResultEntity>>(songs);
            }

            var albums = _albumServices.SearchByName(query).ToList();
            IList<SearchResultEntity> listAlbumModels = new List<SearchResultEntity>();
            if (albums.Any())
            {
                Mapper.CreateMap<AlbumEntity, SearchResultEntity>()
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail) : ""));
                listAlbumModels = Mapper.Map<IList<AlbumEntity>, IList<SearchResultEntity>>(albums);
            }

            var artists = _artistServices.SearchByName(query).ToList();
            IList<SearchResultEntity> listArtistModels = new List<SearchResultEntity>();
            if (artists.Any())
            {
                Mapper.CreateMap<ArtistEntity, SearchResultEntity>()
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !string.IsNullOrEmpty(albs.Thumbnail) ? Url.Content( ALBUM_IMAGE_PATH + albs.Thumbnail) : ""));
                listArtistModels = Mapper.Map<IList<ArtistEntity>, IList<SearchResultEntity>>(artists);
            }

            SearchResult result = new SearchResult()
            {
                Songs = listSongModels,
                Albums = listAlbumModels,
                Artists = listArtistModels
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
        #endregion
    }
}
