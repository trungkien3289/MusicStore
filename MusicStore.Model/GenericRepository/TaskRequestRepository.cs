using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.GenericRepository
{
    public class TaskRequestRepository : GenericRepository<fl_TaskRequest>
    {
        public TaskRequestRepository(DbContext context): base(context)
        {

        }

        public IEnumerable<fl_TaskRequest> Get(int projectId, int userId, int page = 1, int numberItemsPerPage = 10)
        {
            var taskRequests = (from tr in Context.Set<fl_TaskRequest>()
                                  join trd in Context.Set<fl_TaskRequestDeveloper>()
                                  on tr.Id equals trd.TaskRequestId
                                  where tr.ProjectId == projectId && trd.UserId == userId
                                  select new fl_TaskRequest
                                  {
                                      Id = tr.Id,
                                      TaskId = tr.TaskId,
                                      Status = tr.Status,
                                      AssigneeId = tr.AssigneeId,
                                      Description = tr.Description,
                                      ProjectId = tr.ProjectId
                                  }).Skip((page - 1) * numberItemsPerPage).Take(numberItemsPerPage).ToList();

            return taskRequests;
        }

        public IEnumerable<fl_TaskRequest> Get(int projectId, int page = 1, int numberItemsPerPage = 10)
        {
            var taskRequests = DbSet.Where(tr => tr.ProjectId == projectId).Skip((page - 1) * numberItemsPerPage).Take(numberItemsPerPage).ToList();
            return taskRequests;
        }
    }
}
