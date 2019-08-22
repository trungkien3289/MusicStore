using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class TaskEntity
    {
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

        public UserEntity Assignee { get; set; }
    }
}
