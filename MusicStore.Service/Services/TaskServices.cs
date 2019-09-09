using AutoMapper;
using Helper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.MessageModels;
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
                fl_Task entity = new fl_Task()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Status = (int)TaskStatusEnum.New,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    EstimatedTime = model.EstimatedTime,
                    ProjectId = model.ProjectId,
                };

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

		public IEnumerable<TaskEntity> GetAll()
		{
			var tasks = _unitOfWork.TaskRepository.GetAll().ToList();
			if (tasks.Any())
			{
				var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Task, TaskEntity>());
				var mapper = config.CreateMapper();
				var model = mapper.Map<List<fl_Task>, List<TaskEntity>>(tasks);
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

        public int Count(int userId)
        {
            return _unitOfWork.TaskRepository.Count(t => t.AssigneeId == userId);
        }

        public GetTasksResponseModel GetTasks(GetTasksRequestModel request, int userId)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<GetTasksRequestModel, GetTasksWithFiltersRequest>());
            var mapper = config.CreateMapper();
            var requestModel = mapper.Map<GetTasksRequestModel, GetTasksWithFiltersRequest>(request);
            var result = _unitOfWork.TaskRepository.GetWithFilters(requestModel, userId);

            var configResponse = new MapperConfiguration(cfg => cfg.CreateMap<GetTasksWithFiltersResponse, GetTasksResponseModel>());
            var mapperResponse = config.CreateMapper();
            var response = mapper.Map<GetTasksWithFiltersResponse, GetTasksResponseModel>(result);

            return response;
        }
        #endregion
    }
}

