using BusinessServices;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Helpers;
using System.Security.Cryptography;
using System.Globalization;

namespace WebApplication.Controllers
{
    /// <summary>
    /// User operations services.
    /// </summary>
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        private readonly IUserServices _userService;

        private static System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();

        public UserController()
        {
            this._userService = new UserServices();
        }

        /// <summary>
        /// Authenticate user by email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>true if user exists, false otherwise</returns>
        [HttpPost]
        [Route("authenticate")]
        public HttpResponseMessage Authenticate([FromBody] User user)
        {
            bool isAuthenticated = false;

            if (user != null)
            {
                isAuthenticated = _userService.Authenticate(user.Email, user.Password);
            }

            return Request.CreateResponse(HttpStatusCode.OK, isAuthenticated);
        }

        /// <summary>
        /// Get request authorization.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true is the request is authorized, false otherwise</returns>
        [HttpGet]
        [Route("confidentials/{email}")]
        public HttpResponseMessage Authorize(string email)
        {
            var request = Request;

            if (request.GetHeaderValue("Authorization") != null && request.GetHeaderValue("Authorization").StartsWith("AWS "))
            {
                // Get AWS signature V2 request credential
                string[] credential = request.GetHeaderValue("Authorization").Substring(4).Split(':');

                // Get user secret accessKey
                string accessKey = _userService.GetAccessKey(email);

                if (accessKey != null && credential[1].Count() > 1)
                {
                    // Request date must be within 15 minutes old
                    DateTime clientDate;
                    if (request.GetHeaderValue("X-Amz-Date") != null
                        && DateTime.TryParseExact(request.GetHeaderValue("X-Amz-Date"), "R", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AdjustToUniversal, out clientDate)
                        && Math.Abs((clientDate - DateTime.UtcNow).TotalMinutes) <= 15)
                    {
                        var signature = GetSignature(request, accessKey);

                        if (signature == credential[1])
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, true);
                        }
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.Unauthorized, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey">Optional</param>
        /// <returns></returns>
        [HttpGet]
        [Route("generateapikey/{accessKey?}")]
        public HttpResponseMessage GenerateApiKey(string accessKey="")
        {
            var request = Request;

            if (string.IsNullOrEmpty(accessKey))
            {
                accessKey = ApiKeyHelper.GenerateAccessKey();
            }

            return Request.CreateResponse(HttpStatusCode.OK, String.Format("AWS {0}:{1}", accessKey, GetSignature(request, accessKey)));
        }

        /// <summary>
        /// Get request signature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        private string GetSignature(HttpRequestMessage request, string accessKey)
        {
            var stringToSign = String.Format("{0}\n{1}\n{2}\n{3}\n{4}",
                                request.Method,
                                request.GetHeaderValue("Content-MD5") ?? "",
                                request.GetHeaderValue("Content-Type") ?? "",
                                request.GetHeaderValue("X-Amz-Date") ?? "",
                                "/johnsmith/photos/puppy.jpg");

            var hmac = new HMACSHA1(utf8.GetBytes(accessKey));
            return Convert.ToBase64String(hmac.ComputeHash(utf8.GetBytes(stringToSign)));
        }
    }
}
