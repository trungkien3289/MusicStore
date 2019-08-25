using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.ActionFilters;
using WebAPI.Filters;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [AuthorizationRequiredAttributeMVC]
    public class TaskRequestController : Controller
    {

        private IProjectServices _projectServices;
        private ITaskServices _taskServices;
        private ITaskRequestServices _taskRequestServices;

        public TaskRequestController(IProjectServices projectServices,
            ITaskServices taskServices,
            ITaskRequestServices taskRequestServices
        )
        {
            _projectServices = projectServices;
            _taskServices = taskServices;
            _taskRequestServices = taskRequestServices;
        }

        // GET: TaskRequest
        public ActionResult Index()
        {
            // get current user
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            // get projects
            TaskRequestViewModel viewModel = new TaskRequestViewModel()
            {
                projectWithTasks = _projectServices.GetByUserId(user.UserId),
            };
            return View(viewModel);
        }
    }
}