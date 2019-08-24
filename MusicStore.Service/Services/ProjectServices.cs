using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace MusicStore.Service.Services
{
    public class ProjectServices : IProjectServices
    {
        #region variables
        private readonly UnitOfWork _unitOfWork;

        #endregion

        #region constructors
        public ProjectServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region public functions

        /// <summary>
        /// Get project details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectEntity GetById(int id)
        {
            var entity = _unitOfWork.ProjectRepository.GetByID(id);
            if (entity != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Project, ProjectEntity>()
                     .ForMember(p => p.Leaders, opt => opt.Ignore())
                     .ForMember(p => p.Developers, opt => opt.Ignore())
                     .ForMember(p => p.TaskRequests, opt => opt.Ignore())
                     .ForMember(p => p.Tasks, opt => opt.Ignore())
                     );
                var mapper = config.CreateMapper();
                var model = mapper.Map<fl_Project, ProjectEntity>(entity);
                return model;
            }
            return null;
        }

        /// <summary>
        /// Get project details with developers and leaders
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectEntity GetProjectDetailsById(int id)
        {
            var entity = _unitOfWork.ProjectRepository.GetByID(id);
            if (entity != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Project, ProjectEntity>()
                     .ForMember(p => p.TaskRequests, opt => opt.Ignore())
                     .ForMember(p => p.Tasks, opt => opt.Ignore())
                     );
                var mapper = config.CreateMapper();
                var model = mapper.Map<fl_Project, ProjectEntity>(entity);
                return model;
            }
            return null;
        }

        /// <summary>
        /// Get projects of user(user is admin or leader or developer)
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        public IEnumerable<ProjectEntity> GetByUserId(int id)
        {
            var projects = _unitOfWork.ProjectRepository.GetProjectByUserId(id);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Project, ProjectEntity>());
            var mapper = config.CreateMapper();
            var results = mapper.Map<IEnumerable<fl_Project>, IEnumerable<ProjectEntity>>(projects);
            return results;
        }

        public IEnumerable<ProjectEntity> GetWithTaskRequestByUserId(int id)
        {
            var projects = _unitOfWork.ProjectRepository.GetProjectWithTaskRequestByUserId(id);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Project, ProjectEntity>());
            var mapper = config.CreateMapper();
            var results = mapper.Map<IEnumerable<fl_Project>, IEnumerable<ProjectEntity>>(projects);
            return results;
        }

        public IEnumerable<TaskEntity> GetTasks(int id)
        {
            var project = _unitOfWork.ProjectRepository.GetWithInclude(a => a.Id == id, "Tasks").FirstOrDefault();
            if (project != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Task, TaskEntity>());
                var mapper = config.CreateMapper();
                var tasks = mapper.Map<List<fl_Task>, List<TaskEntity>>(project.Tasks.ToList());
                return tasks;
            }
            return null;
        }

        public IEnumerable<ProjectEntity> GetAll()
        {
            var projects = _unitOfWork.ProjectRepository.GetAll().ToList();
            if (projects.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Project, ProjectEntity>());
                var mapper = config.CreateMapper();
                var results = mapper.Map<List<fl_Project>, List<ProjectEntity>>(projects);
                return results;
            }
            return null;
        }

        public ProjectEntity Create(ProjectEntity model)
        {
            using (var scope = new TransactionScope())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ProjectEntity, fl_Project> ());
                var mapper = config.CreateMapper();
                var entity = mapper.Map<ProjectEntity, fl_Project>(model);
                _unitOfWork.ProjectRepository.Insert(entity);
                _unitOfWork.Save();
                scope.Complete();
                model.Id = entity.Id;
                return model;
            }
        }

        public ProjectEntity Update(int id, ProjectEntity model)
        {
            using (var scope = new TransactionScope())
            {
                var project = _unitOfWork.ProjectRepository.GetByID(id);
                project.Name = model.Name;
                project.Description = model.Description;
                project.Status = model.Status;
                _unitOfWork.Save();
                scope.Complete();
            }

            return model;
        }

        public bool Delete(int id)
        {
            var success = false;
            if (id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var project = _unitOfWork.ProjectRepository.GetByID(id);
                    if (project != null)
                    {
                        _unitOfWork.ProjectRepository.Delete(project);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }

            return success;
        }

        public IEnumerable<ProjectEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 10)
        {
            var projects = _unitOfWork.ProjectRepository.GetWithInclude(a => a.Name.Contains(query)).OrderBy(a => a.Name).Skip(--page * numberItemsPerPage).Take(numberItemsPerPage).ToList();
            if (projects.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Project, ProjectEntity>());
                var mapper = config.CreateMapper();
                var results = mapper.Map<List<fl_Project>, List<ProjectEntity>>(projects);
                return results;
            }

            return null;
        }

        public IEnumerable<ProjectEntity> Get(int page = 1, int numberItemsPerPage = 10)
        {
            var projects = _unitOfWork.ProjectRepository.GetAll().OrderBy(a => a.Name).Skip(--page * numberItemsPerPage).Take(numberItemsPerPage).ToList();
            if (projects.Any())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<fl_Project, ProjectEntity>());
                var mapper = config.CreateMapper();
                var results = mapper.Map<List<fl_Project>, List<ProjectEntity>>(projects);
                return results;
            }

            return null;
        }

        /// <summary>
        /// Get number of projects, which user join as leader or developer
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int  Count(int userId)
        {
           int totalProjects = _unitOfWork.ProjectRepository.CountProjectByUserId(userId);
           return totalProjects;
        }

        #endregion
    }
}
