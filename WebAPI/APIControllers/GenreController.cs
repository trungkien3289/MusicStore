using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.APIControllers
{
    public class GenreController : ApiController
    {
        #region variables
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
        private readonly IGenreServices _genreServices;

        #endregion

        public GenreController(IGenreServices genreServices)
        {
            _genreServices = genreServices;
        }

        // GET api/genre
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get()
        {
            var genres = _genreServices.GetAllGenres();
            if (genres != null)
            {
                var genreEntities = genres as List<GenreEntity> ?? genres.ToList();
                if (genreEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, genreEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Genres not found");
        }

        public HttpResponseMessage GetAlbumsOfGenre(int id)
        {
            var albums = _genreServices.GetAlbumsOfGenre(id).ToList();
            var albumEntities = new List<AlbumEntity>();
            if (albums != null)
            {
                if (albums.Any())
                {
                    Mapper.CreateMap<AlbumEntity, AlbumEntity>().ForMember(a => a.Thumbnail, map => map.MapFrom(al => Url.Content(ALBUM_IMAGE_PATH + al.Thumbnail)));
                    albumEntities = Mapper.Map<List<AlbumEntity>, List<AlbumEntity>>(albums);
                }
                return Request.CreateResponse(HttpStatusCode.OK, albumEntities);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Genre not found.");
            }
        }

        public HttpResponseMessage GetSongsOfGenre(int id)
        {
            var songs = _genreServices.GetSongsOfGenre(id).ToList();
            var songEntities = new List<SongEntity>();
            if (songs != null)
            {
                if (songs.Any())
                {
                    Mapper.CreateMap<SongEntity, SongEntity>()
                        .ForMember(a => a.Thumbnail, map => map.MapFrom(al => Url.Content(ALBUM_IMAGE_PATH + al.Thumbnail)))
                        .ForMember(a => a.MediaUrl, map => map.MapFrom(al => Url.Content(SONG_BASE_PATH + al.MediaUrl)));
                    songEntities = Mapper.Map<List<SongEntity>, List<SongEntity>>(songs);
                }
                return Request.CreateResponse(HttpStatusCode.OK, songEntities);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Genre not found.");
            }
        }

        public HttpResponseMessage GetArtistsOfGenre(int id)
        {
            var artists = _genreServices.GetArtistsOfGenre(id).ToList();
            var artistEntities = new List<ArtistEntity>();
            if (artists != null)
            {
                if (artists.Any())
                {
                    Mapper.CreateMap<ArtistEntity, ArtistEntity>()
                        .ForMember(a => a.Thumbnail, map => map.MapFrom(al => Url.Content(ALBUM_IMAGE_PATH + al.Thumbnail)));
                    artistEntities = Mapper.Map<List<ArtistEntity>, List<ArtistEntity>>(artists);
                }
                return Request.CreateResponse(HttpStatusCode.OK, artistEntities);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Genre not found.");
            }
        }
    }
}
