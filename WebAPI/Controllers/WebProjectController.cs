using AutoMapper;
using Helper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class WebProjectController : Controller
    {

        private IProjectServices _projectServices;

        public WebProjectController(IProjectServices projectService)
        {
            _projectServices = projectService;
        }
        // GET: Project
        public ActionResult Index(int page=1)
        {
            var projects = _projectServices.Get(page, Constants.PAGE_SIZE).ToList();
           
            ViewBag.page = page;

            return View(projects);
        }

        /// <summary>
        /// Return Create view to let user input information of new project
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create a project
        /// </summary>
        /// <param name="productRequest">information of new project</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ProjectEntity project)
        {
            if (ModelState.IsValid)
            {
                // Add product
                //_projectServices.Create(project);
                return RedirectToAction("Index");
            }
            return View(project);
        }
    }
}