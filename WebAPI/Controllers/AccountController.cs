using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserServices _userService;
        private readonly ITokenServices _tokenServices;
        public AccountController(IUserServices userService, ITokenServices tokenServices)
        {
            _userService = userService;
            _tokenServices = tokenServices;
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}