using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebAPI.Controllers
{

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
	}
}