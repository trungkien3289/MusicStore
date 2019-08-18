using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IService
{
    public interface ITaskRequestServices
    {
        TaskRequestEntity GetById(int id);
        IEnumerable<TaskRequestEntity> Get(int projectId, int userId, int page = 1, int numberItemsPerPage = 10);
        TaskRequestEntity Create(TaskRequestEntity model);
        TaskRequestEntity Update(int id, TaskRequestEntity model);
        bool Delete(int id);
        void ApproveTaskRequest(int userId, int taskRequestId);
        void JoinTaskRequest(int taskRequestId, int userId);
    }
}
