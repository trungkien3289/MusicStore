using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class fl_TaskRequest
    {
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Task Status New| Active|Close
        /// </summary>
        public int Status { get; set; }
        public Nullable<int> AssigneeId { get; set; }

        public virtual system_User Assignee { get; set; }
        public virtual fl_Task Task { get; set; }
        public virtual ICollection<fl_TaskRequestDeveloper> Developers { get; set; }
        public virtual ICollection<fl_RequestComment> RequestComments { get; set; }
    }
}
