using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using MidiDomoticaApi.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomoticaApi.Filters
{
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Method)]
    public class AuthorizedAttribute : TypeFilterAttribute
    {
        public AuthorizedAttribute() : base(typeof(AuthorizedFilter))
        {
            Arguments = new object[] { };
        }
    }

    public class AuthorizedFilter : IAuthorizationFilter
    {
        private const string _accessTokenHeaderName = "MidiDomotica_AccessToken";

        public AuthorizedFilter()
        {
            
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            StringValues accessTokens = context.HttpContext.Request.Headers[_accessTokenHeaderName];

            string sourceIp = context.HttpContext.Request.Headers.ContainsKey("X-Real-Ip") ? context.HttpContext.Request.Headers["X-Real-Ip"]
                            : context.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For") ? context.HttpContext.Request.Headers["X-Forwarded-For"]
                            : $"Unknown source via {context.HttpContext.Connection.RemoteIpAddress}:{context.HttpContext.Connection.RemotePort}";

            if (accessTokens.Count < 1 && Regex.IsMatch(context.HttpContext.Request.Path.Value, "/api/photos/webcontent/[0-9]+") && !context.HttpContext.Request.Query.TryGetValue(_accessTokenHeaderName, out accessTokens))
            {
                Logger.Log($"Denied request from {sourceIp}. Reason: No access_token provided!");
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    ContentType = "text/html",
                    Content = "<html><body>No access_token provided!\n<a href=\"http://192.168.2.101:55554/Login" + "\">Login here</a></body></html>",
                };
                return;
            }
           string accessToken = accessTokens.ToString().Replace(' ', '+');

            if (!AuthManager.ValidateToken(accessToken))
            {
                Logger.Log($"Denied request from {sourceIp}. Reason: Invalid access_token provided!");
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    ContentType = "text/html",
                    Content = "<html><body>Invalid access_token provided!\nRefresh your token or <a href=\"http://192.168.2.101:55554/Login" + "\">Login here</a></body></html>",
                };
                return;
            }
        }
    }
}
