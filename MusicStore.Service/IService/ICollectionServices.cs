using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IService
{
    public interface ICollectionServices
    {
        IEnumerable<CollectionEntity> GetAllCollections();
        CollectionEntity GetCollectionById(int collectionId);
        IEnumerable<CollectionEntity> GetFeaturedCollections();
        IEnumerable<SongEntity> GetSongsOfCollection(int collectionId);
        IEnumerable<CollectionEntity> GetCollectionsAfterBeginCharacter(string character, int page, int pAGE_SIZE);
    }
}
