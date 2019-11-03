using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Middlewares
{
    /// <summary>
    /// <seealso cref="https://medium.com/@tarik.nzl/asp-net-core-and-signalr-authentication-with-the-javascript-client-d46c15584daf"/>
    /// </summary>
    public class WebSocketsMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;

            // web sockets cannot pass headers so we must take the access token from query param and
            // add it to the header before authentication middleware runs
            if (request.Path.StartsWithSegments("/hubs", StringComparison.OrdinalIgnoreCase) &&
                request.Query.TryGetValue("access_token", out var accessToken))
            {
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }

            await _next(httpContext);
        }
    }
}