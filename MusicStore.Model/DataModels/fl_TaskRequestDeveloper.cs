using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.DataModels
{
    public class fl_TaskRequestDeveloper
    {
        public int Id { get; set; }
        public int TaskRequestId { get; set; }
        public int UserId { get; set; }
        public bool IsJoin { get; set; }

        public virtual fl_TaskRequest TaskRequest { get; set; }
        public virtual system_User User { get; set; }
    }
}
