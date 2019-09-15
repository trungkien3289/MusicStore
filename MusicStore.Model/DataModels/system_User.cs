using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class system_User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public string Photo { get; set; }

        public virtual system_UserRole Role { get; set; }
        public virtual ICollection<fl_Task> Tasks { get; set; }
        //public virtual ICollection<fl_TaskRequest> DeveloperTaskRequests { get; set; }
        public virtual ICollection<fl_TaskRequest> AssigneeTaskRequests { get; set; }
        public virtual ICollection<fl_Project> LeaderProjects { get; set; }
        public virtual ICollection<fl_Project> DeveloperProjects { get; set; }
        public virtual ICollection<fl_RequestComment> RequestComments{ get; set; }
    }
}
