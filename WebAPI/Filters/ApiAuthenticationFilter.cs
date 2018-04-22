using MusicStore.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;

namespace WebAPI.Filters
{
    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {

        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            var provider = actionContext.ControllerContext.Configuration
                               .DependencyResolver.GetService(typeof(IUserServices)) as IUserServices;

            if(provider != null)
            {
                var userId = provider.Authenticate(username, password);
                if (userId > 0)
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if(basicAuthenticationIdentity != null)
                    {
                        basicAuthenticationIdentity.UserId = userId;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}