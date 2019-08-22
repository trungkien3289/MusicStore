using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI.Controllers
{
    public class ProjectController : Controller
    {
		private IProjectServices _projectServices;

		public ProjectController(IProjectServices projectServices)
		{
			this._projectServices = projectServices;
		}
		// GET: Project
		public ActionResult Index()
		{
			// get projects
			var viewModel = _projectServices.GetAll();
			return View(viewModel);
		}
	}
}