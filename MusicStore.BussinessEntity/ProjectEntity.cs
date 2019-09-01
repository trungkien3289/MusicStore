using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Project Status New| Inprogress|Done
        /// </summary>
        public int Status { get; set; }

        public ICollection<TaskRequestEntity> TaskRequests { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; }
        public ICollection<UserEntity> Leaders { get; set; }
        public ICollection<UserEntity> Developers { get; set; }
    }

    public class GetProjectWithTaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Project Status New| Inprogress|Done
        /// </summary>
        public int Status { get; set; }

        public ICollection<GetProjectWithTaskResponse_Task> Tasks { get; set; }
    }

    public class GetProjectWithTaskResponse_Task
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

    public class GetProjectWithTaskRequestResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Project Status New| Inprogress|Done
        /// </summary>
        public int Status { get; set; }

        public ICollection<GetProjectWithTaskRequestResponse_TaskRequest> TaskRequests { get; set; }
    }

    public class GetProjectWithTaskRequestResponse_TaskRequest
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
        public virtual TaskEntity Task { get; set; }
    }
}
