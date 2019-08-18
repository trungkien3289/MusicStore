using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<ProjectEntity> projectWithTasks { get; set; }
        public IEnumerable<ProjectEntity> projectWithRequestTasks { get; set; }
    }
}