using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using MusicStore.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class WebCollectionController : Controller
    {
        private const string ALBUM_IMAGE_PATH = @"~/Data/AlbumImages/";
        private const string SONG_PATH = @"~/Data/Album_Songs/";
        private const string ARTIST_IMAGE_PATH = @"~/Data/Artist_Images/";
        private const string COLLECTION_IMAGE_PATH = @"~/Data/Collection_Images/";
        private readonly IAlbumServices _albumServices;
        private readonly IArtistServices _artistServices;
        private readonly ICollectionServices _collectionServices;
        private const int PAGE_SIZE = 20;

        public WebCollectionController(IAlbumServices albumServices, IArtistServices artistServices, ICollectionServices collectionServices)
        {
            _albumServices = albumServices;
            _artistServices = artistServices;
            _collectionServices = collectionServices;
        }
        // GET: Album
        public ActionResult Details(int id)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            CollectionDetailsViewModel collectionViewModel = new CollectionDetailsViewModel();
            var collection = _collectionServices.GetCollectionById(id);
            if (collection != null)
            {
                IList<SongEntity> listSongsOfCollection = _collectionServices.GetSongsOfCollection(id).ToList();

                Mapper.CreateMap<SongEntity, SongEntity>()
                    .ForMember(ae => ae.MediaUrl, map => map.MapFrom(albs => DomainName + Url.Content(SONG_PATH + albs.MediaUrl)));
                collectionViewModel.Songs = Mapper.Map<IList<SongEntity>, IList<SongEntity>>(listSongsOfCollection);

                Mapper.CreateMap<CollectionEntity, CollectionEntity>()
                    .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => DomainName + Url.Content(ALBUM_IMAGE_PATH + albs.Thumbnail)));
                collectionViewModel.CollectionDetail = Mapper.Map<CollectionEntity, CollectionEntity>(collection);

                var featuredCollections = _collectionServices.GetFeaturedCollections();
                IList<CollectionEntity> listCollectionModels = new List<CollectionEntity>();
                if (featuredCollections != null)
                {
                    Mapper.CreateMap<CollectionEntity, CollectionEntity>()
                        .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(COLLECTION_IMAGE_PATH + albs.Thumbnail) : String.Empty));
                    listCollectionModels = Mapper.Map<IList<CollectionEntity>, IList<CollectionEntity>>(featuredCollections.ToList());
                }

                collectionViewModel.TopCollections = listCollectionModels;

                return View(collectionViewModel);
            }

            return RedirectToAction("Error", "Home");

        }
        public ActionResult Index(string character = "a", int page = 1)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            CollectionViewModel returnModel = new CollectionViewModel();
            var collections = _collectionServices.GetCollectionsAfterBeginCharacter(character, page, PAGE_SIZE);
            Mapper.CreateMap<CollectionEntity, CollectionEntity>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(COLLECTION_IMAGE_PATH + albs.Thumbnail) : String.Empty));
            if (collections != null)
            {
                returnModel.Collections = Mapper.Map<IList<CollectionEntity>, IList<CollectionEntity>>(collections.ToList());
            }

            var featuredArtists = _artistServices.GetFeaturedArtists().ToList();
            Mapper.CreateMap<ArtistEntity, ArtistEntity>().ForMember(s => s.Thumbnail, map => map.MapFrom(ss => !String.IsNullOrEmpty(ss.Thumbnail) ? Url.Content(ARTIST_IMAGE_PATH + ss.Thumbnail): String.Empty));
            returnModel.TopArtists = Mapper.Map<List<ArtistEntity>, List<ArtistEntity>>(featuredArtists);

            ViewBag.SelectedCharacter = character;

            return View(returnModel);
        }

        [HttpPost]
        public ActionResult GetPagingCollectionByCharacter(string character = "a", int page = 1)
        {
            string DomainName = Request.Url.Scheme + "://" + Request.Url.Authority;
            var collections = _collectionServices.GetCollectionsAfterBeginCharacter(character, page, PAGE_SIZE);
            Mapper.CreateMap<CollectionEntity, CollectionEntity>()
                .ForMember(ae => ae.Thumbnail, map => map.MapFrom(albs => !String.IsNullOrEmpty(albs.Thumbnail) ? Url.Content(COLLECTION_IMAGE_PATH + albs.Thumbnail) : String.Empty));
            IList<CollectionEntity> result = new List<CollectionEntity>();
            if (collections != null)
            {
                result = Mapper.Map<IList<CollectionEntity>, IList<CollectionEntity>>(collections.ToList());
            }

            return PartialView("_ListCollectionSummaryItem", result);
        }
    }
}