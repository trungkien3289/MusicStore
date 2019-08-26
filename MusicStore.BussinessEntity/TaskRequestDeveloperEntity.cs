using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.BussinessEntity
{
    public class TaskRequestDeveloperEntity
    {
        public int Id { get; set; }
        public int TaskRequestId { get; set; }
        public int UserId { get; set; }
        public bool IsJoin { get; set; }

        public virtual TaskRequestEntity TaskRequest { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
