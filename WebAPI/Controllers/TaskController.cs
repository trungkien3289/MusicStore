using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAPI.ActionFilters;

namespace WebAPI.Controllers
{
    [AuthorizationRequiredAttributeMVC]
    public class TaskController : Controller
	{
		private ITaskServices _taskServices;
		private IProjectServices _projectServices;

		public TaskController(ITaskServices taskServices, IProjectServices projectServices)
		{
			_taskServices = taskServices;
			_projectServices = projectServices;
		}
		// GET: Task
		public ActionResult Index()
		{
			var tasks = _taskServices.GetAll();
			return View(tasks);
		}

		public ActionResult CreateTask()
		{
			var projects = _projectServices.GetAll();
			ViewBag.ProjectId = new SelectList(projects.OrderBy(n => n.Id), "Id", "Name");
			return View();
		}

		[HttpPost]
		public ActionResult CreateTask(TaskEntity task)
		{
			_taskServices.Create(task);
			return RedirectToAction("Index");
		}

		public ActionResult DeleteTask(int id)
		{
			var foundTask = _taskServices.GetById(id);
			if (foundTask != null)
			{
				_taskServices.Delete(id);

			}
			return RedirectToAction("Index");
		}
	}
}