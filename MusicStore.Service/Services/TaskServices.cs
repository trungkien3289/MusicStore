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
    public class TaskServices : ITaskServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;
        #endregion

        #region constructors
        public TaskServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region public functions
        public TaskEntity Create(TaskEntity model)
        {
            using (var scope = new TransactionScope())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TaskEntity, fl_Task>());
                var mapper = config.CreateMapper();
                var entity = mapper.Map<TaskEntity, fl_Task>(model);
                _unitOfWork.TaskRepository.Insert(entity);
                _unitOfWork.Save();
                scope.Complete();
                model.Id = entity.Id;
                return model;
            }
        }

        public bool Delete(int id)
        {
            var success = false;
            if (id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var task = _unitOfWork.TaskRepository.GetByID(id);
                    if (task != null)
                    {
                        _unitOfWork.TaskRepository.Delete(task);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }

            return success;
        }

        public IEnumerable<TaskEntity> Get(int? projectId, int? userId, int? status, int page = 1, int numberItemsPerPage = 10)
        {
            var tasks = _unitOfWork.TaskRepository.Get(projectId, userId, status, page, numberItemsPerPage).ToList();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Task, TaskEntity>());
            var mapper = config.CreateMapper();
            var results = mapper.Map<IEnumerable<fl_Task>, IEnumerable<TaskEntity>>(tasks);
            return results;
        }

        public TaskEntity GetById(int id)
        {
            var entity = _unitOfWork.TaskRepository.GetByID(id);
            if (entity != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Task, TaskEntity>());
                var mapper = config.CreateMapper();
                var model = mapper.Map<fl_Task, TaskEntity>(entity);
                return model;
            }
            return null;
        }

        public TaskEntity Update(int id, TaskEntity model)
        {
            using (var scope = new TransactionScope())
            {
                var task = _unitOfWork.TaskRepository.GetByID(id);
                task.Name = model.Name;
                task.Description = model.Description;
                task.Status = model.Status;
                task.AssigneeId = model.AssigneeId;
                task.StartDate = model.StartDate;
                task.EndDate = model.EndDate;
                _unitOfWork.Save();
                scope.Complete();
            }

            return model;
        }

        #endregion
    }
}
