using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly UnitOfWork _unitOfWork;
        public AlbumService()
        {
            this._unitOfWork = new UnitOfWork();
        }

        public Models.AlbumSummary GetAlbumById(int albumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.AlbumSummary> GetAllAlbums()
        {
            throw new NotImplementedException();
        }

        public int CreateAlbum(Models.AlbumSummary albumSummary)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAlbum(int albumId, Models.AlbumSummary albumSummary)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAlbum(int albumId)
        {
            throw new NotImplementedException();
        }
    }
}
