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
                Mapper.CreateMap<ms_Genre, GenreEntity>();
                var genresModel = Mapper.Map<List<ms_Genre>, List<GenreEntity>>(genres);
                return genresModel;
            }
            return null;
        }
    }
}
