using AutoMapper;
using Helper;
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
    public class TaskRequestServices : ITaskRequestServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;
        #endregion

        #region constructors
        public TaskRequestServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void ApproveTaskRequest(int userId, int taskRequestId)
        {
            if(IsJoinedTaskRequestDeveloper(userId, taskRequestId))
            {
                var taskRequest = _unitOfWork.TaskRequestRepository.GetByID(taskRequestId);
                taskRequest.AssigneeId = userId;
                taskRequest.Status = (int)TaskRequestStatusEnum.Close;
                _unitOfWork.Save();
            }

            throw new Exception("User does not belong to list joined developer of this task");
        }

        public void JoinTaskRequest(int taskRequestId, int userId)
        {
            if(IsTaskRequestDeveloper(userId, taskRequestId))
            {
                var taskRequestDeveloper = _unitOfWork.TaskRequestDeveloperRepository.GetFirst(
                    trd => trd.UserId == userId 
                    && trd.TaskRequestId == taskRequestId);

                taskRequestDeveloper.IsJoin = true;
                _unitOfWork.Save();
            }

            throw new Exception("User does not belong to list developer of this task");
        }

        private bool IsTaskRequestDeveloper(int userId, int taskRequestId)
        {
            var taskRequestDeveloper = _unitOfWork.TaskRequestDeveloperRepository.Get(
               trd => trd.UserId == userId
               && trd.TaskRequestId == taskRequestId);
            return taskRequestDeveloper != null ? true : false;
        }

        private bool IsJoinedTaskRequestDeveloper(int userId, int taskRequestId)
        {
            var joinedTaskRequestDeveloper = _unitOfWork.TaskRequestDeveloperRepository.Get(
                trd => trd.UserId == userId
                && trd.TaskRequestId == taskRequestId 
                && trd.IsJoin);
            return joinedTaskRequestDeveloper != null ? true : false;
        }

        #endregion

        #region public functions
        public TaskRequestEntity Create(TaskRequestEntity model)
        {
            using (var scope = new TransactionScope())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TaskRequestEntity, fl_TaskRequest>());
                var mapper = config.CreateMapper();
                var entity = mapper.Map<TaskRequestEntity, fl_TaskRequest>(model);
                _unitOfWork.TaskRequestRepository.Insert(entity);
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
                    var taskRequest = _unitOfWork.TaskRequestRepository.GetByID(id);
                    if (taskRequest != null)
                    {
                        _unitOfWork.TaskRequestRepository.Delete(taskRequest);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }

            return success;
        }

        public IEnumerable<TaskRequestEntity> Get(int projectId, int userId, int page = 1, int numberItemsPerPage = 10)
        {
            var user = _unitOfWork.UserRepository.GetByID(userId);
            if (user == null) throw new Exception("User cannot found.");
            IEnumerable<fl_TaskRequest> taksRequests;
            if(IsLeader(projectId, userId) || user.RoleId == (int)UserRoleEnum.ADMIN)
            {
                taksRequests = _unitOfWork.TaskRequestRepository.Get(projectId, page, numberItemsPerPage);
            }
            else
            {
                taksRequests = _unitOfWork.TaskRequestRepository.Get(projectId, userId, page, numberItemsPerPage);
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_TaskRequest, TaskRequestEntity>());
            var mapper = config.CreateMapper();
            var results = mapper.Map<IEnumerable<fl_TaskRequest>, IEnumerable<TaskRequestEntity>>(taksRequests);
            return results;
        }

        public TaskRequestEntity GetById(int id)
        {
            var entity = _unitOfWork.TaskRequestRepository.GetByID(id);
            if (entity != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_TaskRequest, TaskRequestEntity>());
                var mapper = config.CreateMapper();
                var model = mapper.Map<fl_TaskRequest, TaskRequestEntity>(entity);
                return model;
            }
            return null;
        }

        public TaskRequestEntity GetSummary(int id)
        {
            var entity = _unitOfWork.TaskRequestRepository.GetByID(id);
            if (entity != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_TaskRequest, TaskRequestEntity>()
                .ForMember(tr => tr.Project, opt => opt.Ignore())
                );
                var mapper = config.CreateMapper();
                var model = mapper.Map<fl_TaskRequest, TaskRequestEntity>(entity);
                return model;
            }
            return null;
        }

        public TaskRequestEntity Update(int id, TaskRequestEntity model)
        {

            using (var scope = new TransactionScope())
            {
                var task = _unitOfWork.TaskRequestRepository.GetByID(id);
                task.Description = model.Description;
                task.Status = model.Status;
                task.AssigneeId = model.AssigneeId;
                _unitOfWork.Save();
                scope.Complete();
            }

            return model;
        }

        #endregion

        #region private function
        private bool IsLeader(int projectId, int userId)
        {
            var project = _unitOfWork.ProjectRepository.GetFirst(p => p.Id == projectId);
            var leaderIds = project.Leaders.Select(l => l.UserId).ToList();
            return leaderIds.Contains(userId);
        }

        public int Count(int userId)
        {
            return _unitOfWork.TaskRequestRepository.Count(tr => tr.AssigneeId == userId);
        }
        #endregion
    }
}

