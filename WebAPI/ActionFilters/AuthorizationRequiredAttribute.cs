using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAPI.Filters;

namespace WebAPI.ActionFilters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute, IActionFilter
    {
        private const string Token = "Token";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var tokenService = filterContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ITokenServices)) as ITokenServices;
            //Get token for call ajax from website and get token from cookies
            string tokenValue = "";
            CookieHeaderValue cookie = filterContext.Request.Headers.GetCookies("Token").FirstOrDefault();
            if (cookie != null)
            {
                tokenValue = cookie["Token"].Value;
            }
            // Get token for device request with token is keep in request headers
            if (String.IsNullOrEmpty(tokenValue) && filterContext.Request.Headers.Contains(Token))
            {
                tokenValue = filterContext.Request.Headers.GetValues(Token).First();
            }
            //if (filterContext.Request.Headers.Contains(Token))
            //{
            //tokenValue = filterContext.Request.Headers.GetValues(Token).First();
            if (!String.IsNullOrEmpty(tokenValue)) { 
                // Validate Token
                if (tokenService != null && !tokenService.ValidateToken(tokenValue))
                {
                    var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                    {
                        ReasonPhrase = "Invalid Request."
                    };
                    filterContext.Response = responseMessage;
                }

                try
                {
                    UserEntity userInfor = tokenService.getUserInforFromToken(tokenValue);
                    var identity = new BasicAuthenticationIdentity(userInfor.UserName, userInfor.Password)
                    {
                        UserId = userInfor.UserId,
                        RoleId = userInfor.RoleId,
                        Token = tokenValue
                    };
                    var genericPrincipal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = genericPrincipal;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error when get user infor and add to identity");
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}