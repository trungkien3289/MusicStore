using AutoMapper;
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
                Mapper.CreateMap<ms_Collection, CollectionEntity>();
                var collectionsModel = Mapper.Map<List<ms_Collection>, List<CollectionEntity>>(collections);
                return collectionsModel;
            }
            return null;
        }

        public CollectionEntity GetCollectionById(int collectionId)
        {
            var collection = _unitOfWork.CollectionRepository.GetByID(collectionId);
            if (collection != null)
            {
                Mapper.CreateMap<ms_Collection, CollectionEntity>();
                var collectionModel = Mapper.Map<ms_Collection, CollectionEntity>(collection);
                return collectionModel;
            }
            return null;
        }

        public IEnumerable<CollectionEntity> GetFeaturedCollections()
        {
            var featuredCollections = _unitOfWork.CollectionRepository.GetMany(a => a.IsFeatured == true).ToList();
            //var featuredCollections = _unitOfWork.CollectionRepository.GetAll().ToList();
            if (featuredCollections.Any())
            {
                Mapper.CreateMap<ms_Collection, CollectionEntity>();
                var collections = Mapper.Map<List<ms_Collection>, List<CollectionEntity>>(featuredCollections);
                return collections;
            }

            return null;
        }

        public IEnumerable<SongEntity> GetSongsOfCollection(int collectionId)
        {
            var collection = _unitOfWork.CollectionRepository.GetWithInclude(a => a.Id == collectionId, "Songs").FirstOrDefault();
            if (collection != null)
            {
                Mapper.CreateMap<ms_Song, SongEntity>()
                    .ForMember(ae => ae.AlbumId, map => map.MapFrom(albs => collection.Id))
                    .ForMember(ae => ae.AlbumName, map => map.MapFrom(albs => collection.Title))
                    .ForMember(ae => ae.AlbumThumbnail, map => map.MapFrom(albs => collection.Thumbnail))
                    .ForMember(ae => ae.ArtistId, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Id : 0))
                    .ForMember(ae => ae.ArtistName, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Name : String.Empty))
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => albs.Artists.Count() > 0 ? albs.Artists.First().Thumbnail : String.Empty));
                var songs = Mapper.Map<List<ms_Song>, List<SongEntity>>(collection.Songs.ToList());
                return songs;
            }
            return null;
        }

        #endregion
    }
}
