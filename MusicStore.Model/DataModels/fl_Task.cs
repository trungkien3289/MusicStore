using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class fl_Task
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
   
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Task Status New| Inprogress|Completed|Close|FeedBack
        /// </summary>
        public int Status { get; set; }
        public Nullable<int> AssigneeId { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public float EstimatedTime { get; set; }

        public virtual fl_Project Project { get; set; }
        public virtual system_User Assignee { get; set; }
        public virtual fl_TaskRequest TaskRequest { get; set; }
  
    }
}
