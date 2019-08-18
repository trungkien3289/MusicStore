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

            if (user.RoleId == (int)UserRoleEnum.ADMIN)
            {
                return this.DbSet.Select(p => new fl_Project()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Tasks = p.Tasks.Where(t => t.TaskRequest != null).Select(task => new fl_Task()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        Description = task.Description,
                        AssigneeId = task.AssigneeId,
                        Status = task.Status,
                        StartDate = task.StartDate,
                        EndDate = task.EndDate,
                        ProjectId = task.ProjectId,
                        EstimatedTime = task.EstimatedTime,
                    }).ToList()
                }).ToList();
            }
            else
            {
                var tempProjects = this.DbSet.Where(p => p.Leaders.Any(u => u.UserId == userId)
                       || p.Developers.Any(u => u.UserId == userId)).Include("Tasks").ToList();
                var results =
                        tempProjects.Select(p => new fl_Project()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Status = p.Status,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            Tasks = p.Tasks.Where(t => t.TaskRequest != null).Select(task => new fl_Task()
                            {
                                Id = task.Id,
                                Name = task.Name,
                                Description = task.Description,
                                AssigneeId = task.AssigneeId,
                                Status = task.Status,
                                StartDate = task.StartDate,
                                EndDate = task.EndDate,
                                ProjectId = task.ProjectId,
                                EstimatedTime = task.EstimatedTime,
                            }).ToList()
                        }).ToList();

                return results;
            }
        }
    }
}
