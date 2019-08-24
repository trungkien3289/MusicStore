using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.MessageModel
{
    public class DashBoardDataMessageModel
    {
        public int TotalProjects { get; set; }
        public int TotalTasks { get; set; }
        public int TotalTaskRequests { get; set; }
    }
}