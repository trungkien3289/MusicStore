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

    public class CreateUpdateTaskRequestResponse
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public Nullable<int> AssigneeId { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Task Status New| Active|Close
        /// </summary>
        public int Status { get; set; }
        public virtual UserEntity Assignee { get; set; }
        public virtual ICollection<TaskRequestDeveloperSummary> Developers { get; set; }
    }

    public class UpdateTaskRequestRequest
    {
        public UpdateTaskRequestRequest()
        {
            Developers = new List<int>();
        }
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Task Status New| Active|Close
        /// </summary>
        public int Status { get; set; }
        public IList<int> Developers { get; set; }
    }

    public class TaskRequestDeveloperSummary
    {
        public int TaskRequestId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Photo { get; set; }
        public bool IsJoin { get; set; }
        public bool IsAssigned { get; set; }
    }

    public class GetUserTaskRequestDetailsResponse
    {
        public TaskRequestEntity TaskRequestDetails { get; set; }
        public bool IsJoin { get; set; }
    }
}

