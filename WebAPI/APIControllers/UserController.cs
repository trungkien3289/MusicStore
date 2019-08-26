using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Models.MessageModel;

namespace WebAPI.APIControllers
{
    public class UserController : ApiController
    {

        private IUserServices _userServices;


        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [AuthorizationRequiredAttribute]
        [Route("api/developers/available")]
        [HttpGet]
        public HttpResponseMessage GetAllActiveUser()
        {
            var availableDevelopers = _userServices.GetAll().Select(u => new DeveloperItemMessageModel()
            {
                Id = u.UserId,
                Name = u.UserName
            });

            return Request.CreateResponse(HttpStatusCode.OK, availableDevelopers);
        }

    }
}
