using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.GenericRepository
{
    public class TaskRepository : GenericRepository<fl_Task>
    {
        public TaskRepository(DbContext context): base(context)
        {

        }

        public IQueryable<fl_Task> Get(int? projectId, int? userId, int? status, int page = 1, int numberItemsPerPage = 10)
        {
            IQueryable<fl_Task> query = DbSet;
            if (projectId != null)
            {
                query = query.Where(t => t.ProjectId == projectId);
            }

            if (userId != null)
            {
                query = query.Where(t => t.AssigneeId == userId);
            }

            if (status != null)
            {
                query = query.Where(t => t.Status == status);
            }

            return query.Skip((page - 1) * numberItemsPerPage).Take(numberItemsPerPage);
        }
    }
}
