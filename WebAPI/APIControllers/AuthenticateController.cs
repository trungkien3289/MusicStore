using Helper;
using MusicStore.BussinessEntity;
using MusicStore.Service.IService;
using MusicStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Filters;

namespace WebAPI.APIControllers
{
    public class AuthenticateController : ApiController
    {
        #region Private variable.

        private readonly ITokenServices _tokenServices;
        private readonly IUserServices _userService;

        #endregion
        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public AuthenticateController(ITokenServices tokenServices, IUserServices userService)
        {
            _tokenServices = tokenServices;
            _userService = userService;
        }

        #endregion

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns></returns>
        [ApiAuthenticationFilter]
        [Route("login")]
        [Route("authenticate")]
        [Route("get/token")]
        [HttpPost]
        public HttpResponseMessage Authenticate()
        {
            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var userId = basicAuthenticationIdentity.UserId;
                    return GetAuthToken(userId);
                }
            }
            return null;
        }

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns></returns>
        [Route("post/register")]
        [HttpPost]
        public HttpResponseMessage Register([FromBody]UserEntity user)
        {
            ValidateUser(user);

            try
            {
                UserEntity newUser = _userService.RequestRegisterUser(user.UserName, user.Password, user.Email);
                return GetAuthToken(newUser.UserId);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = "Bad request."
                });
            }
        }

        /// <summary>
        /// Returns auth token for the validated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private HttpResponseMessage GetAuthToken(int userId)
        {
            var token = _tokenServices.GenerateToken(userId);
            var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            response.Headers.Add("Token", token.AuthToken);
            response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
            response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");

            var cookie = new CookieHeaderValue("Token", token.AuthToken);
            cookie.Expires = DateTimeOffset.Now.AddSeconds(int.Parse(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";

            response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return response;
        }

        [AuthorizationRequiredAttribute]
        [Route("api/signout")]
        [HttpPost]
        public HttpResponseMessage SignOut()
        {
            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var token = basicAuthenticationIdentity.Token;
                    if (!_tokenServices.Kill(token))
                    {
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("Cannot delete token."),
                            ReasonPhrase = "Cannot delete token."
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
                else
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Cannot found Identity."),
                        ReasonPhrase = "Cannot found Identity."
                    });
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("CurrentPrincipal is null or user is not authenticated."),
                    ReasonPhrase = "CurrentPrincipal is null or user is not authenticated."
                });
            }
        }

        #region private functions
        private void ValidateUser(UserEntity user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("User Name cannot be empty."),
                    ReasonPhrase = "User Name is required."
                });
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Email cannot be empty."),
                    ReasonPhrase = "Email is required."
                });
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Password cannot be empty."),
                    ReasonPhrase = "Password is required."
                });
            }

            if (!Validator.EmailIsValid(user.Email))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Email invalid."),
                    ReasonPhrase = "Email format is not correct."
                });
            }

        }
        #endregion
    }
}
