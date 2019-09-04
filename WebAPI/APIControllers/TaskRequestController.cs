﻿using Helper;
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
using WebAPI.Models.MessageModel;

namespace WebAPI.APIControllers
{
    public class TaskRequestController : ApiController
    {
        private readonly ITaskRequestServices _taskRequestServices;
        private readonly ITaskServices _taskServices;

        public TaskRequestController(ITaskRequestServices taskRequestServices, ITaskServices taskServices)
        {
            _taskRequestServices = taskRequestServices;
            _taskServices = taskServices;
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

        /// <summary>
        /// get user task request details by task id and user id
        /// </summary>
        /// <param name="taskRequestId"></param>
        /// <returns></returns>
        [AuthorizationRequiredAttribute]
        [Route("api/task/requests/{taskRequestId}/byuser")]
        [HttpGet]
        public HttpResponseMessage DetailsByUser([FromUri]int taskRequestId)
        {
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            var taskRequest = _taskRequestServices.GetUserTaskRequestDetails(taskRequestId, user.UserId);
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

        [AuthorizationRequiredAttribute]
        [HttpPut]
        [Route("api/taskrequest/update")]
        public HttpResponseMessage Update(UpdateTaskRequestRequest taskRequest)
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

        /// <summary>
        /// Delete task request by task request Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete task request by task id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/task/{taskId}/taskrequest")]
        [AuthorizationRequiredAttribute]
        public HttpResponseMessage DeleteTaskRequestOfTask([FromUri]int taskId)
        {
            var taskRequest = _taskRequestServices.GetTaskRequestByTaskId(taskId);
            var isSuccess = _taskRequestServices.Delete(taskRequest.Id);
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

        [Route("api/taskrequest/{taskRequestId}/unassigndeveloper")]
        [HttpPost]
        public HttpResponseMessage UnassignTaskRequest(int taskRequestId)
        {
            try
            {
                _taskRequestServices.UnassignTaskRequest(taskRequestId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [AuthorizationRequiredAttribute]
        [HttpPost]
        [Route("api/taskrequest/{taskRequestId}/requestjoin")]
        public HttpResponseMessage JoinTaskRequest([FromUri]int taskRequestId)
        {
            try
            {
                var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                _taskRequestServices.JoinTaskRequest(taskRequestId, user.UserId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}
