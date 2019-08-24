using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Filters;
using WebAPI.Models.MessageModel;

namespace WebAPI.APIControllers
{
    public class DashBoardController : ApiController
    {
        private readonly IProjectServices _projectServices;
        private readonly ITaskServices _taskServices;
        private readonly ITaskRequestServices _taskRequestServices;

        public DashBoardController(
            IProjectServices projectServices,
            ITaskServices taskServices,
            ITaskRequestServices taskRequestServices
            )
        {
            _projectServices = projectServices;
            _taskServices = taskServices;
            _taskRequestServices = taskRequestServices;
        }

        [AuthorizationRequiredAttribute]
        [Route("api/dashboard/data")]
        [HttpGet]
        public HttpResponseMessage DashBoardData()
        {
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            try
            {
                int totalProjects = _projectServices.Count(user.UserId);
                int totalTasks = _taskServices.Count(user.UserId);
                int totalTaskRequests = _taskRequestServices.Count(user.UserId);
                var retModel = new DashBoardDataMessageModel()
                {
                    TotalProjects = totalProjects,
                    TotalTasks = totalTasks,
                    TotalTaskRequests = totalTaskRequests
                };
                return Request.CreateResponse(HttpStatusCode.OK, retModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}
