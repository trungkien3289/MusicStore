using Helper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Filters;

namespace WebAPI.APIControllers
{
    public class TaskController : ApiController
    {
        private readonly ITaskServices _taskServices;
        private readonly ITaskRequestServices _taskRequestServices;
        private readonly IProjectServices _projectServices;

        public TaskController(
            ITaskServices taskServices,
            ITaskRequestServices taskRequestServices,
            IProjectServices projectServices)
        {
            _taskServices = taskServices;
            _taskRequestServices = taskRequestServices;
            _projectServices = projectServices;
        }

        public HttpResponseMessage Get(int? projectId, int? userId, int? status, int page = 1)
        {

            var tasks = _taskServices.Get(projectId, userId, status, page, Constants.NumberItemsPerPage);
            if (tasks != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tasks);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no task found.");
        }

        /// <summary>
        /// get task details by id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [AuthorizationRequiredAttribute]
        [Route("api/tasks/{taskId}")]
        [HttpGet]
        public HttpResponseMessage Details([FromUri]int taskId)
        {
            var task = _taskServices.GetById(taskId);
            if (task != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no task found.");
        }

        [AuthorizationRequiredAttribute]
        [Route("api/tasks/create")]
        [HttpPost]
        public HttpResponseMessage Add(TaskEntity task)
        {
            var createdTask = _taskServices.Create(task);
            if (createdTask != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, createdTask);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on create task");
            }
        }

        [AuthorizationRequiredAttribute]
        [Route("api/tasks/update")]
        [HttpPut]
        public HttpResponseMessage Update(TaskEntity task)
        {
            var createdTask = _taskServices.Update(task.Id, task);
            if (createdTask != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, createdTask);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on create task");
            }
        }

        [AuthorizationRequiredAttribute]
        [Route("api/tasks/{taskId}")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri]int taskId)
        {
            var isSuccess = _taskServices.Delete(taskId);
            if (isSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on delete task");
            }
        }

        [AuthorizationRequiredAttribute]
        [Route("api/tasks/{taskId}/taskrequest")]
        [HttpGet]
        public HttpResponseMessage GetTaskRequestOfTask([FromUri]int taskId)
        {
            var taskRequest = _taskRequestServices.GetTaskRequestOfTask(taskId);
            return Request.CreateResponse(HttpStatusCode.OK, taskRequest);
        }

        [AuthorizationRequiredAttribute]
        [Route("api/tasks")]
        [HttpPost]
        public HttpResponseMessage GetTasks([FromBody]GetTasksRequestModel request)
        {
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            if (request.Filter.Project.IsEnable)
            {
                if (!_projectServices.CheckUserCanGetTasksOfProject(request.Filter.Project.Value,user.UserId, user.RoleId))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User does not have permission to get tasks of project.");
                }
            }
            
            var response = _taskServices.GetTasks(request, user.UserId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
            
        }
    }
}
