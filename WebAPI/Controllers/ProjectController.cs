using MusicStore.BussinessEntity;
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

		public ActionResult CreateProject()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateProject(ProjectEntity project)
		{
			_projectServices.Create(project);
			return RedirectToAction("Index");
		}

		public ActionResult DeleteProject(int id)
		{
			var foundProject = _projectServices.GetById(id);
			if (foundProject != null)
			{
				_projectServices.Delete(id);

			}
			return RedirectToAction("Index");
		}
	}
}