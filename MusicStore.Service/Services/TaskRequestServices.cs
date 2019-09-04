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

        private bool IsTaskRequestDeveloper(int userId, int taskRequestId)
        {
            var taskRequestDeveloper = _unitOfWork.TaskRequestDeveloperRepository.Get(
               trd => trd.UserId == userId
               && trd.TaskRequestId == taskRequestId);
            return taskRequestDeveloper != null ? true : false;
        }
        private bool IsJoinedTaskRequestDeveloper(int userId, int taskRequestId)
        {
            var joinedTaskRequestDeveloper = _unitOfWork.TaskRequestDeveloperRepository.Count(
                trd => trd.UserId == userId
                && trd.TaskRequestId == taskRequestId
                && trd.IsJoin);
            return joinedTaskRequestDeveloper > 0 ? true : false;
        }

        private bool IsTaskRequestClose(int taskRequestId)
        {
            var taskRequest = _unitOfWork.TaskRequestRepository.GetByID(taskRequestId);
            if (taskRequest == null) throw new Exception("Task Request Not found");
            return (taskRequest.Status == (int)TaskRequestStatusEnum.Close);
        }

        private bool IsTaskRequestAssigned(int taskRequestId)
        {
            var taskRequest = _unitOfWork.TaskRequestRepository.GetByID(taskRequestId);
            if (taskRequest == null) throw new Exception("Task Request Not found");
            return (taskRequest.AssigneeId != null);
        }

        #endregion

        #region public functions
        public TaskRequestEntity Create(CreateTaskRequestRequest model)
        {
            using (var scope = new TransactionScope())
            {
                var task = _unitOfWork.TaskRepository.GetByID(model.TaskId);
                if (task == null) throw new Exception("Task not found.");
                if (task.TaskRequest != null) throw new Exception("Task Request already exist in Task.");
                var project = _unitOfWork.ProjectRepository.GetByID(model.ProjectId);
                if (project == null) throw new Exception("Project not found.");
                fl_TaskRequest newTaskRequest = new fl_TaskRequest()
                {
                    Description = model.Description,
                    Status = model.Status,
                    Task = task,
                    Project = project,
                    Developers = new List<fl_TaskRequestDeveloper>()
                };

                if (model.Developers.Count() > 0)
                {
                    foreach (var id in model.Developers)
                    {
                        newTaskRequest.Developers.Add(new fl_TaskRequestDeveloper()
                        {
                            IsJoin = false,
                            UserId = id
                        });
                    }
                }

                _unitOfWork.TaskRequestRepository.Insert(newTaskRequest);
                _unitOfWork.Save();
                scope.Complete();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_TaskRequest, TaskRequestEntity>()
                    .ForMember(p => p.Project, opt => opt.Ignore())
                );
                var mapper = config.CreateMapper();
                var returnModel = mapper.Map<fl_TaskRequest, TaskRequestEntity>(newTaskRequest);
                return returnModel;
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

        public TaskRequestEntity Update(int id, UpdateTaskRequestRequest model)
        {

            using (var scope = new TransactionScope())
            {
                var taskRequest = _unitOfWork.TaskRequestRepository.GetByID(id);
                taskRequest.Description = model.Description;
                taskRequest.Status = model.Status;
                var cloneListDevelopers = new List<fl_TaskRequestDeveloper>(taskRequest.Developers);

                // Delete all developers of task request
                foreach (var dev in cloneListDevelopers)
                {
                    _unitOfWork.TaskRequestDeveloperRepository.Delete(d => d.Id == dev.Id);

                }
                taskRequest.Developers.Clear();

                foreach (var devId in model.Developers)
                {
                    var taskRequestDeveloper = new fl_TaskRequestDeveloper()
                    {
                        IsJoin = false,
                        UserId = devId
                    };
                    var oldDev = cloneListDevelopers.FirstOrDefault(d => d.UserId == devId);
                    if (oldDev != null)
                    {
                        taskRequestDeveloper.IsJoin = oldDev.IsJoin;
                    }
                    taskRequest.Developers.Add(taskRequestDeveloper);
                }
                _unitOfWork.Save();

                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_TaskRequest, TaskRequestEntity>()
                .ForMember(tr => tr.Project, opt => opt.Ignore())
                );
                var mapper = config.CreateMapper();
                var returnModel = mapper.Map<fl_TaskRequest, TaskRequestEntity>(taskRequest);
                scope.Complete();

                return returnModel;
            }
        }

        public CreateUpdateTaskRequestResponse GetTaskRequestOfTask(int taskId)
        {
            var task = _unitOfWork.TaskRepository.GetFirst(t => t.Id == taskId);
            if(task.TaskRequest!= null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_TaskRequest, CreateUpdateTaskRequestResponse>()
                .ForMember(m => m.Developers, opt => opt.Ignore()));
                var mapper = config.CreateMapper();
                var model = mapper.Map<fl_TaskRequest, CreateUpdateTaskRequestResponse>(task.TaskRequest);

                model.Developers = new List<TaskRequestDeveloperSummary>();
                foreach (var dev in task.TaskRequest.Developers)
                {
                    model.Developers.Add(new TaskRequestDeveloperSummary()
                    {
                        UserId = dev.UserId,
                        UserName = dev.User.UserName,
                        TaskRequestId = task.TaskRequest.Id,
                        IsJoin = dev.IsJoin,
                        IsAssigned = dev.UserId == task.AssigneeId? true: false
                    });
                }

                return model;
            }
            else
            {
                return null;
            }
           
        }

        public GetUserTaskRequestDetailsResponse GetUserTaskRequestDetails(int taskRequestId, int userId)
        {
            var entity = _unitOfWork.TaskRequestRepository.GetByID(taskRequestId);
            if (entity == null) throw new Exception("Task Request Not Found.");
            if (!entity.Developers.Any(dev => dev.UserId == userId)) throw new Exception("User is not belong to list target deeloper of task request.");

            var result = new GetUserTaskRequestDetailsResponse();
            result.IsJoin = entity.Developers.First(d => d.UserId == userId).IsJoin;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_TaskRequest, TaskRequestEntity>()
            .ForMember(tr => tr.Project, opt => opt.Ignore())
            );
            var mapper = config.CreateMapper();
            result.TaskRequestDetails = mapper.Map<fl_TaskRequest, TaskRequestEntity>(entity);
            return result;
        }

        public void ApproveTaskRequest(int userId, int taskRequestId)
        {
            if (IsTaskRequestClose(taskRequestId) && IsTaskRequestAssigned(taskRequestId)) throw new Exception("Task Request is close or assigned.");
            if (IsJoinedTaskRequestDeveloper(userId, taskRequestId))
            {
                var taskRequest = _unitOfWork.TaskRequestRepository.GetByID(taskRequestId);
                taskRequest.AssigneeId = userId;
                taskRequest.Status = (int)TaskRequestStatusEnum.Close;
                taskRequest.Task.AssigneeId = userId;
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception("User does not belong to list joined developer of this task");
            }
        }

        public void JoinTaskRequest(int taskRequestId, int userId)
        {
            if (IsTaskRequestClose(taskRequestId) && IsTaskRequestAssigned(taskRequestId)) throw new Exception("Task Request is close or assigned.");
            if (IsTaskRequestDeveloper(userId, taskRequestId))
            {
                var taskRequestDeveloper = _unitOfWork.TaskRequestDeveloperRepository.GetFirst(
                    trd => trd.UserId == userId
                    && trd.TaskRequestId == taskRequestId);

                taskRequestDeveloper.IsJoin = true;
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception("User does not belong to list developer of this task");
            }
        }

        public void UnassignTaskRequest(int taskRequestId)
        {
            if (IsTaskRequestClose(taskRequestId)) throw new Exception("Task Request is close");
            var taskRequest = _unitOfWork.TaskRequestRepository.GetByID(taskRequestId);
            taskRequest.AssigneeId = null;
            taskRequest.Task.AssigneeId = null;
            _unitOfWork.Save();
        }

        public TaskRequestEntity GetTaskRequestByTaskId(int taskId)
        {
            var task = _unitOfWork.TaskRepository.GetFirst(t => t.Id == taskId);
            if (task.TaskRequest == null)
            {
                throw new Exception("Task Request not existed.");
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_TaskRequest, TaskRequestEntity>()
            .ForMember(tr => tr.Assignee, opt => opt.Ignore())
            .ForMember(tr => tr.Project, opt => opt.Ignore())
            .ForMember(tr => tr.Task, opt => opt.Ignore())
            );
            var mapper = config.CreateMapper();
            var model = mapper.Map<fl_TaskRequest, TaskRequestEntity>(task.TaskRequest);

            return model;
        }
        #endregion
    }
}

