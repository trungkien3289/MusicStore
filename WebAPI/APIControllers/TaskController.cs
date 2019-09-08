using Helper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.APIControllers
{
    public class TaskController : ApiController
    {
        private readonly ITaskServices _taskServices;
        private readonly ITaskRequestServices _taskRequestServices;

        public TaskController(ITaskServices taskServices, ITaskRequestServices taskRequestServices)
        {
            _taskServices = taskServices;
            _taskRequestServices = taskRequestServices;
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

        [HttpPost]
        [Route("api/task")]
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

        [HttpPut]
        [Route("api/task")]
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

        [HttpDelete]
        [Route("api/task/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var isSuccess = _taskServices.Delete(id);
            if (isSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on delete task");
            }
        }

        [Route("api/tasks/{taskId}/taskrequest")]
        [HttpGet]
        public HttpResponseMessage GetTaskRequestOfTask([FromUri]int taskId)
        {
            var taskRequest = _taskRequestServices.GetTaskRequestOfTask(taskId);
            return Request.CreateResponse(HttpStatusCode.OK, taskRequest);
        }
    }
}
