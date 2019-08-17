using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class fl_Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Project Status New| Inprogress|Done
        /// </summary>
        public int Status { get; set; }

        public virtual ICollection<fl_Task> Tasks { get; set; }
        public virtual ICollection<system_User> Leaders { get; set; }
        public virtual ICollection<system_User> Developers { get; set; }
    }
}
