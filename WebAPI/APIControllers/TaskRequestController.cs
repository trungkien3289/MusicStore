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
using WebAPI.Models.MessageModel;

namespace WebAPI.APIControllers
{
    public class TaskRequestController : ApiController
    {
        private readonly ITaskRequestServices _taskRequestServices;

        public TaskRequestController(ITaskRequestServices taskRequestServices)
        {
            _taskRequestServices = taskRequestServices;
        }

        public HttpResponseMessage Get(int projectId, int userId, int? status, int page = 1)
        {

            var taskRequests = _taskRequestServices.Get(projectId, userId, page, Constants.NumberItemsPerPage);
            if (taskRequests != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, taskRequests);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no task request found.");
        }

        /// <summary>
        /// get task request details by id
        /// </summary>
        /// <param name="taskRequestId"></param>
        /// <returns></returns>
        [Route("api/task/requests/{taskRequestId}")]
        [HttpGet]
        public HttpResponseMessage Details([FromUri]int taskRequestId)
        {
            var taskRequest = _taskRequestServices.GetSummary(taskRequestId);
            if (taskRequest != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, taskRequest);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no task request found.");
        }

        [AuthorizationRequiredAttribute]
        [HttpPost]
        [Route("api/taskrequest/create")]
        public HttpResponseMessage Add(CreateTaskRequestRequest taskRequest)
        {
            var createdTaskRequest = _taskRequestServices.Create(taskRequest);
            if (createdTaskRequest != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, createdTaskRequest);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on create task request");
            }
        }

        public HttpResponseMessage Update(TaskRequestEntity taskRequest)
        {
            var createdRequestTask = _taskRequestServices.Update(taskRequest.Id, taskRequest);
            if (createdRequestTask != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, createdRequestTask);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on create task request");
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            var isSuccess = _taskRequestServices.Delete(id);
            if (isSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on delete task request");
            }
        }

        [Route("api/taskrequest/{taskRequestId}/pickdeveloper/{userId}")]
        [HttpPost]
        public HttpResponseMessage ApproveTaskRequest(int taskRequestId, int userId)
        {
            try
            {
                _taskRequestServices.ApproveTaskRequest(userId, taskRequestId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public HttpResponseMessage JoinTaskRequest(int userId, int taskRequestId)
        {
            try
            {
                _taskRequestServices.JoinTaskRequest(userId, taskRequestId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}
