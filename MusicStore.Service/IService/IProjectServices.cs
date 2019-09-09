﻿using MusicStore.BussinessEntity;
using System.Collections.Generic;

namespace MusicStore.Service.IService
{
    public interface IProjectServices
    {
        ProjectEntity GetById(int id);
        ProjectEntity GetProjectDetailsById(int id);
        IEnumerable<ProjectEntity> GetByUserId(int id);
        IEnumerable<ProjectEntity> GetForLeader(int id);
        IEnumerable<ProjectEntity> GetWithTaskRequestByUserId(int id);
        IEnumerable<TaskEntity> GetTasks(int id);
        IEnumerable<ProjectEntity> GetAll();
        IEnumerable<ProjectEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 10);
        IEnumerable<ProjectEntity> Get(int page = 1, int numberItemsPerPage = 10);
        ProjectEntity Create(ProjectRequest model);
        ProjectEntity Update(int id, ProjectRequest model);
        bool Delete(int id);
        int Count(int userId);
        bool CheckUserCanGetTasksOfProject(int projectId, int userId, int roleId);
        IEnumerable<ProjectFilterItem> GetProjectFilterItemsForUser(int id);
    }
}
