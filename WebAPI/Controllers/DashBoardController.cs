using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DashBoardController : Controller
    {
        private IProjectServices _projectServices;

        public DashBoardController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            // get current user
            var userId = 2;
            // get projects
            var viewModel = new DashboardViewModel()
            {
                projectWithTasks = _projectServices.GetByUserId(userId),
                projectWithRequestTasks = _projectServices.GetWithTaskRequestByUserId(userId)
            };
            return View(viewModel);
        }
    }
}