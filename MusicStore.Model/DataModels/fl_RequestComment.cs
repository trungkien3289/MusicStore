using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class fl_RequestComment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskRequestId { get; set; }
        public string Content { get; set; }

        public virtual system_User User { get; set; }
        public virtual fl_TaskRequest TaskRequest { get; set; }
    }
}
