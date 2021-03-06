﻿using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Services
{
    public class GenreServices : IGenreServices
    {

        private readonly UnitOfWork _unitOfWork;
        #region constructors
        public GenreServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion

        public IEnumerable<GenreEntity> GetAllGenres()
        {
            var genres = _unitOfWork.GenreRepository.GetAll().ToList();
            if (genres.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Genre, GenreEntity>()
                .ForMember(a => a.Slug, map => map.MapFrom(al => al.Url)));
                var mapper = config.CreateMapper();
                var genresModel = mapper.Map<List<ms_Genre>, List<GenreEntity>>(genres);
                return genresModel;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetAlbumsOfGenre(int id)
        {
            var genre = _unitOfWork.GenreRepository.GetSingleWithInclude(g => g.Id == id, "Albums");
            List<AlbumEntity> albumEntities = new List<AlbumEntity>();
            if (genre != null)
            {
                if (genre.Albums.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Album, AlbumEntity>());
                    var mapper = config.CreateMapper();
                    albumEntities = mapper.Map<List<ms_Album>, List<AlbumEntity>>(genre.Albums.ToList());
                    return albumEntities;
                }
                else
                {
                    return albumEntities;
                }
            }
            return null;
        }

        public IEnumerable<SongEntity> GetSongsOfGenre(int id)
        {
            var genre = _unitOfWork.GenreRepository.GetWithInclude(g => g.Id == id, "Songs.Albums", "Songs.Artists").FirstOrDefault();

            List<SongEntity> songEntities = new List<SongEntity>();
            if (genre != null)
            {
                if (genre.Songs.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
                     .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Thumbnail : String.Empty))
                     .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => String.Empty))
                       .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => String.Empty))
                       .ForMember(ae => ae.Description, map => map.MapFrom(albs => String.Empty))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty))
                    .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Id : 0)));
                    var mapper = config.CreateMapper();
                    songEntities = mapper.Map<List<ms_Song>, List<SongEntity>>(genre.Songs.ToList());
                    return songEntities;
                }
                else
                {
                    return songEntities;
                }
            }
            return null;
        }

        public IEnumerable<ArtistEntity> GetArtistsOfGenre(int id)
        {
            var genre = _unitOfWork.GenreRepository.GetSingleWithInclude(g => g.Id == id, "Artists");
            List<ArtistEntity> artistEntities = new List<ArtistEntity>();
            if (genre != null)
            {
                if (genre.Artists.Any())
                {

                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Artist, ArtistEntity>());
                    var mapper = config.CreateMapper();
                    artistEntities = mapper.Map<List<ms_Artist>, List<ArtistEntity>>(genre.Artists.ToList());
                    return artistEntities;
                }
                else
                {
                    return artistEntities;
                }
            }
            return null;
        }

        public GenreDetails GetGenreDetails(int id)
        {
            var genre = _unitOfWork.GenreRepository.GetSingleWithInclude(g => g.Id == id, "Artists", "Songs", "Songs.Artists", "Songs.Albums");

            List<ArtistEntity> artistEntities = new List<ArtistEntity>();
            List<SongEntity> songEntities = new List<SongEntity>();
            if (genre != null)
            {
                var configMapperArtist = new MapperConfiguration(cfg => cfg.CreateMap<ms_Artist, ArtistEntity>()
                .ForMember(ae => ae.NumberOfSongs, map => map.MapFrom(albs => albs.Songs.Count())));
                var mapperArtist = configMapperArtist.CreateMapper();

                if (genre.Artists.Any())
                {
                    var artists = genre.Artists.Where(a => a.Songs.Count() > 0).ToList();
                    artistEntities = mapperArtist.Map<List<ms_Artist>, List<ArtistEntity>>(artists);
                }

                if (genre.Songs.Any())
                {
                    var artists = genre.Artists.Where(a => a.Songs.Count() > 0).ToList();
                    artistEntities = mapperArtist.Map<List<ms_Artist>, List<ArtistEntity>>(artists);
                    List<ms_Song> songs = new List<ms_Song>();
                    foreach (var artist in genre.Artists)
                    {
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
                       .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => artist.Thumbnail))
                       .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => String.Empty))
                       .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => String.Empty))
                       .ForMember(ae => ae.Description, map => map.MapFrom(albs => String.Empty))
                       .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => artist.Name))
                       .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => artist.Id)));
                        var mapper = config.CreateMapper();
                        if (artist.Songs.Any())
                        {
                            songEntities = mapper.Map<List<ms_Song>, List<SongEntity>>(artist.Songs.ToList());
                        }
                    }
                }

                GenreDetails result = new GenreDetails()
                {
                    Artists = artistEntities,
                    Songs = songEntities,
                    Name = genre.Name
                };

                return result;
            }

            return null;
        }

        private ms_Album GetAlbumOfSong(int songId)
        {
            return _unitOfWork.GetAlbumOfSong(songId);
        }

        public IEnumerable<ArtistEntity> GetArtistsOfGenre(int id, int page, int pagesize)
        {
            var genre = _unitOfWork.GenreRepository.GetWithInclude(g => g.Id == id, "Artists").FirstOrDefault();

            List<ArtistEntity> artistEntities = new List<ArtistEntity>();
            if (genre != null)
            {
                if (genre.Artists.Any())
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Artist, ArtistEntity>());
                    var mapper = config.CreateMapper();
                    artistEntities = mapper.Map<List<ms_Artist>, List<ArtistEntity>>(genre.Artists.OrderBy(g => g.Name).Skip((page - 1) * pagesize).Take(pagesize).ToList());
                    return artistEntities;
                }
                else
                {
                    return artistEntities;
                }
            }
            return null;
        }
    }
}
