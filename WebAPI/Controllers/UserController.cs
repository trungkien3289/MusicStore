using MusicStore.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI.Controllers
{
    public class UserController : Controller
    {
		private IUserServices _userServices;

		public UserController(IUserServices userServices)
		{
			this._userServices = userServices;
		}
		// GET: User
		public ActionResult Index()
        {
			var users = _userServices.GetAll();
            return View(users);
        }
    }
}