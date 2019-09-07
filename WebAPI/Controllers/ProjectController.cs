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
		private ITaskServices _taskServices;

		public ProjectController(ITaskServices taskServices)
		{
			_taskServices = taskServices;
		}

        // GET: Project
        public ActionResult Index()
		{
            return View();
		}

		public ActionResult Detail(int id)
		{
			var tasks = _taskServices.Get(id, null, null);
			return View(tasks);
		}
	}
}