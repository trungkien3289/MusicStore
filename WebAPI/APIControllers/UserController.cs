using AutoMapper;
using Helper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Filters;
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
                UserId = u.UserId,
                UserName = u.UserName
            });

            return Request.CreateResponse(HttpStatusCode.OK, availableDevelopers);
        }

        [AuthorizationRequiredAttribute]
        [Route("api/users/profile")]
        [HttpGet]
        public HttpResponseMessage GetUserProfile()
        {
            // get current user
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            UserEntity foundUser = _userServices.GetById(user.UserId);

            return Request.CreateResponse(HttpStatusCode.OK, foundUser);
        }

        [AuthorizationRequiredAttribute]
        [Route("api/users/profile")]
        [HttpPut]
        public HttpResponseMessage UpdateUserProfile(UpdateUserProfileRequest userModel)
        {
            // get current user
            UserEntity foundUser = _userServices.GetById(userModel.UserId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UpdateUserProfileRequest, UserEntity>());
            var mapper = config.CreateMapper();
            var editProfile = mapper.Map<UpdateUserProfileRequest, UserEntity>(userModel);
            try
            {
                var entityResult =  _userServices.UpdateProfile(editProfile);
                return Request.CreateResponse(HttpStatusCode.OK, entityResult);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [AuthorizationRequiredAttribute]
        [Route("api/users/updatephoto")]
        [HttpPost]
        public HttpResponseMessage UpdateUserPhoto(UpdateUserPhotoRequest photo)
        {
            // get current user
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            try
            {
                var resultPhoto = _userServices.UpdateUserPhoto(user.UserId, photo.Photo);
                return Request.CreateResponse(HttpStatusCode.OK, resultPhoto);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,ex.Message);
            }
        }

        [AuthorizationRequiredAttribute]
        [Route("api/users/resetphoto")]
        [HttpPut]
        public HttpResponseMessage RemoveUserPhoto()
        {
            // get current user
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            try
            {
                var defaultPhoto = _userServices.UpdateUserPhoto(user.UserId, Constants.DEFAULT_USER_PHOTO);
                return Request.CreateResponse(HttpStatusCode.OK, defaultPhoto);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [AuthorizationRequiredAttribute]
        [Route("api/users/changepassword")]
        [HttpPut]
        public HttpResponseMessage ChangePassword(ChangePasswordModel requestModel)
        {

            if(requestModel.NewPassword != requestModel.ConfirmNewPassword)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "New password and Confirm new password does not matched.");
            }
            // get current user
            var user = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            try
            {
                _userServices.ChangePassword(user.UserId, requestModel.OldPassword, requestModel.NewPassword);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
