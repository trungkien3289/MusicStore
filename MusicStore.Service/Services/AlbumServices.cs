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
using System.Transactions;

namespace MusicStore.Service.Services
{
    public class AlbumServices : IAlbumServices
    {
        private readonly UnitOfWork _unitOfWork;
        public AlbumServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public AlbumEntity GetAlbumById(int albumId)
        {
            var album = _unitOfWork.AlbumRepository.GetByID(albumId);
            if (album != null)
            {
                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albumModel = Mapper.Map<ms_Album, AlbumEntity>(album);
                return albumModel;
            }
            return null;
        }

        public IEnumerable<AlbumEntity> GetAllAlbums()
        {

            var albums = _unitOfWork.AlbumRepository.GetAll().ToList();
            if (albums.Any())
            {
                Mapper.CreateMap<ms_Album, AlbumEntity>();
                var albumsModel = Mapper.Map<List<ms_Album>, List<AlbumEntity>>(albums);
                return albumsModel;
            }
            return null;
        }

        public int CreateAlbum(AlbumEntity albumSummary)
        {
            using (var scope = new TransactionScope())
            {
                var album = new ms_Album()
                {
                    Title = albumSummary.Title
                };
                _unitOfWork.AlbumRepository.Insert(album);
                _unitOfWork.Save();
                scope.Complete();

                return album.Id;
            }
        }

        public bool UpdateAlbum(int albumId, AlbumEntity albumSummary)
        {
            var success = false;
            if (albumSummary != null)
            {
                using (var scope = new TransactionScope())
                {
                    var album = _unitOfWork.AlbumRepository.GetByID(albumId);
                    album.Title = albumSummary.Title;
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
            }

            return success;
        }

        public bool DeleteAlbum(int albumId)
        {
            var success = false;
            if(albumId>0){
                using (var scope = new TransactionScope())
                {
                    var album = _unitOfWork.AlbumRepository.GetByID(albumId);
                    if (album != null)
                    {
                        _unitOfWork.AlbumRepository.Delete(album);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }

            return success;
        }
    }
}
