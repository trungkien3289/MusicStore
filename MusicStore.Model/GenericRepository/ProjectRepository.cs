using Helper;
using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.GenericRepository
{
    public class ProjectRepository : GenericRepository<fl_Project>
    {
        public ProjectRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<fl_Project> GetProjectByUserId(int userId)
        {
            var user = this.Context.Set<system_User>().Where(u => u.UserId == userId).FirstOrDefault();
            if(user == null)
            {
                throw new Exception("UserCannotFound");
            }

            if(user.RoleId == (int)UserRoleEnum.ADMIN)
            {
                return GetAll();
            }
            else
            {
                return this.DbSet
                    .Where(p => p.Leaders.Any(u => u.UserId == userId) 
                    || p.Developers.Any(u => u.UserId == userId)).Include("Tasks").ToList();
            }
        }

        public IEnumerable<fl_Project> GetProjectWithTaskRequestByUserId(int userId)
        {
            var user = this.Context.Set<system_User>().Where(u => u.UserId == userId).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("UserCannotFound");
            }

            var taskRequests = this.Context.Set<fl_TaskRequest>().Where(tr => tr.Developers.Any(d => d.Id == userId));
            var projectIds = taskRequests.GroupBy(tr => tr.ProjectId).Select(gr => gr.Key).ToList();
            var projects = this.Context.Set<fl_Project>().Where(p => projectIds.Contains(p.Id)).ToList();
            foreach (var project in projects)
            {
                project.TaskRequests = taskRequests.Where(tr => tr.ProjectId == project.Id).ToList();
            }

            return projects;
        }
    }
}
