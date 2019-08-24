using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System.Web.Mvc;
using WebAPI.ActionFilters;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [AuthorizationRequiredMVCController]
    public class ProjectController : Controller
	{
		private IProjectServices _projectServices;
		private ITaskServices _taskServices;

		public ProjectController(IProjectServices projectServices, ITaskServices taskServices)
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

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(ProjectEntity project)
		{
			_projectServices.Create(project);
			return RedirectToAction("Index");
		}

		public ActionResult Delete(int id)
		{
			var foundProject = _projectServices.GetById(id);
			if (foundProject != null)
			{
				_projectServices.Delete(id);

			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Update(int id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}

			var project = _projectServices.GetById(id);

			return View(project);
		}

		[HttpPost]
		public ActionResult Update(ProjectEntity project)
		{
			var updateProject = _projectServices.GetById(project.Id);
			if (updateProject != null)
			{
				updateProject.Name = project.Name;
				updateProject.Description = project.Description;
				updateProject.StartDate = project.StartDate;
				updateProject.EndDate = project.EndDate;

				_projectServices.Update(updateProject.Id, updateProject);
			}


			return RedirectToAction("Index");
		}

		public ActionResult Detail(int projectId)
		{
			var tasks = _taskServices.Get(projectId, 1, 10);
			return View(tasks);
		}
	}
}