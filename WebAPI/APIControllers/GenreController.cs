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
    public class GenreController : ApiController
    {
        #region variables
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private const string SONG_BASE_PATH = @"~/Data/Album_Songs/";
        private const string GENRE_BASE_PATH = @"~/Data/Genre_Images/";
        private readonly IGenreServices _genreServices;

        #endregion

        public GenreController(IGenreServices genreServices)
        {
            _genreServices = genreServices;
        }

        // GET api/genre
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("genres.json")]
        public HttpResponseMessage Get()
        {
            var genres = _genreServices.GetAllGenres();
            IList<GenreEntity> listGenres = new List<GenreEntity>();
            if (genres != null)
            {
                var genreEntities = genres as List<GenreEntity> ?? genres.ToList();
                if (genreEntities.Any())
                {
                    Mapper.CreateMap<GenreEntity, GenreEntity>()
                        .ForMember(a => a.Thumbnail, map => map.MapFrom(al => Url.Content(GENRE_BASE_PATH + al.Thumbnail)));
                    listGenres = Mapper.Map<List<GenreEntity>, List<GenreEntity>>(genreEntities);
                }
            }

            GetAllGenresResponse result = new GetAllGenresResponse()
            {
                Genres = listGenres
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);
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

        [Route("genre-artists-songs.json")]
        [Route("api/Genre/GetSongsAndArtistsOfGenre")]
        public HttpResponseMessage GetSongsAndArtistsOfGenre(int genreid)
        {
            GenreDetails result = _genreServices.GetGenreDetails(genreid);

            if(result != null)
            {
                if (result.Songs.Any())
                {
                    Mapper.CreateMap<SongEntity, SongEntity>()
                         .ForMember(a => a.AlbumThumbnail, map => map.MapFrom(al => !String.IsNullOrEmpty(al.AlbumThumbnail) ? Url.Content(ALBUM_IMAGE_PATH + al.AlbumThumbnail) : String.Empty))
                        .ForMember(a => a.Thumbnail, map => map.MapFrom(al => !String.IsNullOrEmpty(al.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + al.Thumbnail) : String.Empty))
                        .ForMember(a => a.MediaUrl, map => map.MapFrom(al => !String.IsNullOrEmpty(al.MediaUrl) ? Url.Content(SONG_BASE_PATH + al.MediaUrl): String.Empty));
                    result.Songs = Mapper.Map<List<SongEntity>, List<SongEntity>>(result.Songs.ToList());
                }

                if (result.Artists.Any())
                {
                    Mapper.CreateMap<ArtistEntity, ArtistEntity>()
                        .ForMember(a => a.Thumbnail, map => map.MapFrom(al => !String.IsNullOrEmpty(al.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + al.Thumbnail) : String.Empty));
                    result.Artists = Mapper.Map<List<ArtistEntity>, List<ArtistEntity>>(result.Artists.ToList());
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Genre not found.");
            }
        }
    }
}
