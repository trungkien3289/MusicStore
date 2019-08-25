using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class TaskRequestViewModel
    {
        public IEnumerable<ProjectEntity> projectWithTasks { get; set; }
    }
}