using MusicStore.BussinessEntity;
using System.Collections.Generic;

namespace MusicStore.Service.IService
{
    public interface IProjectServices
    {
        ProjectEntity GetById(int id);
        IEnumerable<TaskEntity> GetTasks(int id);
        IEnumerable<ProjectEntity> GetAll();
        IEnumerable<ProjectEntity> SearchByName(string query, int page = 1, int numberItemsPerPage = 10);
        IEnumerable<ProjectEntity> Get(int page = 1, int numberItemsPerPage = 10);
        int Create(ProjectEntity model);
        bool Update(int id, ProjectEntity model);
        bool Delete(int id);
    }
}
