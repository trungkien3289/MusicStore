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

        public ICollection<TaskEntity> Tasks { get; set; }
        public ICollection<UserEntity> Leaders { get; set; }
        public ICollection<UserEntity> Developers { get; set; }
    }
}
