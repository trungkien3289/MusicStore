using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.MessageModels
{
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

        public virtual ICollection<fl_TaskRequest> TaskRequests { get; set; }
    }

    public class GetProjectWithTaskRequestResponse_TaskRequest
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Task Status New| Active|Close
        /// </summary>
        public int Status { get; set; }
        public Nullable<int> AssigneeId { get; set; }

        public virtual fl_Task Task { get; set; }
    }
}
