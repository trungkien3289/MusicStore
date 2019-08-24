using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IService
{
    public interface ITaskServices
    {
        TaskEntity GetById(int id);
        IEnumerable<TaskEntity> Get(int? projectId,int? userId,int? status, int page = 1, int numberItemsPerPage = 10);
        TaskEntity Create(TaskEntity model);
        TaskEntity Update(int id, TaskEntity model);
        bool Delete(int id);
        int Count(int userId);
    }
}
