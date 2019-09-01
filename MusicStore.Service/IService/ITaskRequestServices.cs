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
        TaskRequestEntity GetSummary(int id);
        IEnumerable<TaskRequestEntity> Get(int projectId, int userId, int page = 1, int numberItemsPerPage = 10);
        TaskRequestEntity Create(CreateTaskRequestRequest model);
        TaskRequestEntity Update(int id, UpdateTaskRequestRequest model);
        bool Delete(int id);
        void ApproveTaskRequest(int userId, int taskRequestId);
        void UnassignTaskRequest(int taskRequestId);
        void JoinTaskRequest(int taskRequestId, int userId);
        int Count(int userId);
        CreateUpdateTaskRequestResponse GetTaskRequestOfTask(int taskId);
        GetUserTaskRequestDetailsResponse GetUserTaskRequestDetails(int taskRequestId, int userId);
    }
}

