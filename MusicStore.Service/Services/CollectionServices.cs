﻿using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicStore.Service.Services
{
    public class CollectionServices : ICollectionServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;

        #endregion

        #region constructors
        public CollectionServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region public functions
        public IEnumerable<CollectionEntity> GetAllCollections()
        {
            var collections = _unitOfWork.CollectionRepository.GetAll().ToList();
            if (collections.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Collection, CollectionEntity>());
                var mapper = config.CreateMapper();
                var collectionsModel = mapper.Map<List<ms_Collection>, List<CollectionEntity>>(collections);
                return collectionsModel;
            }
            return null;
        }

        public CollectionEntity GetCollectionById(int collectionId)
        {
            var collection = _unitOfWork.CollectionRepository.GetByID(collectionId);
            if (collection != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Collection, CollectionEntity>());
                var mapper = config.CreateMapper();
                var collectionModel = mapper.Map<ms_Collection, CollectionEntity>(collection);
                return collectionModel;
            }
            return null;
        }

        public IEnumerable<CollectionEntity> GetCollectionsAfterBeginCharacter(string character, int page, int pagesize)
        {
            IList<CollectionEntity> result = new List<CollectionEntity>();
            IList<ms_Collection> collections;
            if (character.Trim().Equals("0-9"))
            {
                collections = _unitOfWork.CollectionRepository.GetWithInclude(a => Regex.IsMatch(a.Title, @"^\d"), "Songs").OrderBy(a => a.Title).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
            else
            {
                collections = _unitOfWork.CollectionRepository.GetWithInclude(a => a.Title.StartsWith(character), "Songs").OrderBy(a => a.Title).Skip((page - 1) * pagesize).Take(pagesize).ToList();
            }
            if (collections != null && collections.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Collection, CollectionEntity>());
                var mapper = config.CreateMapper();
                result = mapper.Map<IList<ms_Collection>, IList<CollectionEntity>>(collections);
                return result;
            }

            return result;
        }

        public IEnumerable<CollectionEntity> GetFeaturedCollections()
        {
            var featuredCollections = _unitOfWork.CollectionRepository.GetMany(a => a.IsFeatured == true).ToList();
            //var featuredCollections = _unitOfWork.CollectionRepository.GetAll().ToList();
            if (featuredCollections.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Collection, CollectionEntity>());
                var mapper = config.CreateMapper();
                var collections = mapper.Map<List<ms_Collection>, List<CollectionEntity>>(featuredCollections);
                return collections;
            }

            return null;
        }

        public IEnumerable<SongEntity> GetSongsOfCollection(int collectionId)
        {
            var collection = _unitOfWork.CollectionRepository.GetWithInclude(a => a.Id == collectionId, "Songs").FirstOrDefault();
            if (collection != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Song, SongEntity>()
                .ForMember(ae => ae.AlbumId, map => map.MapFrom(albs => collection.Id))
                    .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => collection.Title))
                    .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => collection.Thumbnail))
                    .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Id : 0))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty))
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Thumbnail : String.Empty)));
                var mapper = config.CreateMapper();
                var songs = mapper.Map<List<ms_Song>, List<SongEntity>>(collection.Songs.ToList());
                return songs;
            }
            return null;
        }

        #endregion
    }
}
