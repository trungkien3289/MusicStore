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
                return this.DbSet.Where(p => p.Leaders.Contains(user) || p.Developers.Contains(user)).ToList();
            }
        }
    }
}
