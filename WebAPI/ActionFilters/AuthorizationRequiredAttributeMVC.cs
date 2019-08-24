using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebAPI.Filters;

namespace WebAPI.ActionFilters
{
    public class AuthorizationRequiredAttributeMVC : ActionFilterAttribute, IActionFilter
    {
        private const string Token = "Token";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var tokenService = DependencyResolver.Current.GetService(typeof(ITokenServices)) as ITokenServices;
            var url = filterContext.HttpContext.Request.Url.ToString();
            var returnUrl = string.Empty;
            if (url.Contains("login"))
            {
                returnUrl = "/";
            }
            else
            {
                returnUrl = url;
            }
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies["Token"];
            if (cookie != null)
            {
                var tokenValue = cookie.Value;

                // Validate Token
                if (tokenService != null && !tokenService.ValidateToken(tokenValue))
                {
                    filterContext.Result = new RedirectResult("/Account/login?returnUrl=" + returnUrl);
                }

                try
                {
                    UserEntity userInfor = tokenService.getUserInforFromToken(tokenValue);
                    var identity = new BasicAuthenticationIdentity(userInfor.UserName, userInfor.Password)
                    {
                        UserId = userInfor.UserId,
                        RoleId = userInfor.RoleId,
                    };
                    var genericPrincipal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = genericPrincipal;
                }
                catch(Exception ex)
                {
                    throw new Exception("Error when get user infor and add to identity");
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Account/login?returnUrl=" + returnUrl);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}