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
    public class ApplicationServices : IApplicationServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;

        #endregion

        #region constructors
        public ApplicationServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region public functions
        public IEnumerable<ApplicationEntity> GetAllApplications()
        {
            var applications = _unitOfWork.ApplicationRepository.GetAll();

            if (applications != null && applications.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ms_Application, ApplicationEntity>());
                var mapper = config.CreateMapper();
                var listApplication = mapper.Map<List<ms_Application>, List<ApplicationEntity>>(applications.ToList());
                return listApplication;
            }

            return null;
        }

        #endregion
    }
}
