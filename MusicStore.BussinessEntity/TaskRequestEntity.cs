using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class TaskRequestEntity
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Task Status New| Active|Close
        /// </summary>
        public int Status { get; set; }
        public Nullable<int> AssigneeId { get; set; }
        public virtual ProjectEntity Project { get; set; }
        public virtual UserEntity Assignee { get; set; }
        public virtual TaskEntity Task { get; set; }
        //public virtual ICollection<TaskRequestDeveloperEntity> Developers { get; set; }
    }

    public class CreateTaskRequestRequest
    {
        public CreateTaskRequestRequest()
        {
            Developers = new List<int>();
        }
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Task Status New| Active|Close
        /// </summary>
        public int Status { get; set; }
        public IList<int> Developers { get; set; }
    }
}

