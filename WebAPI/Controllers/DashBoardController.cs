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
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            // get projects
            var viewModel = new DashboardViewModel()
            {
                projectWithTasks = _projectServices.GetByUserId(user.UserId),
                projectWithRequestTasks = _projectServices.GetWithTaskRequestByUserId(user.UserId)
            };

            return View(viewModel);
        }
    }
}