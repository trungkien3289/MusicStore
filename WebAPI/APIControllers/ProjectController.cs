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
    public class ProjectController : ApiController
    {
        private readonly IProjectServices _projectServices;

        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        public HttpResponseMessage GetByUser(int id)
        {
            try
            {
                var projects = _projectServices.GetByUserId(id);
                return Request.CreateResponse(HttpStatusCode.OK, projects);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [Route("api/projects/{page}")]
        public HttpResponseMessage GetWithPaging([FromUri]int page = 1) {

            var projects = _projectServices.Get(page);
            if (projects != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, projects);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Albums not found");
        }


        [Route("api/projects/{projectId}")]
        public HttpResponseMessage GetById([FromUri]int projectId)
        {
            var project = _projectServices.GetProjectDetailsById(projectId);
            if (project != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, project);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Project not found");
        }

		[HttpPost]
		[Route("api/addProject")]
		public HttpResponseMessage Add(ProjectEntity project)
        {
            var createdProject = _projectServices.Create(project);
            if(createdProject != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, createdProject);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on create project");
            }
        }

        public HttpResponseMessage Update(ProjectEntity project)
        {
            var createdProject = _projectServices.Update(project.Id, project);
            if (createdProject != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, createdProject);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on create project");
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            var isSuccess = _projectServices.Delete(id);
            if (isSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error on delete project");
            }
        }
    }
}
