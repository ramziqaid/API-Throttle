using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Api
{
    public class ThrottleAttribute : IActionFilter
    { 
   
        private IMemoryCache _cache;
        public string Name { get; set; }

        /// <summary>
        /// The number of seconds clients must wait before executing this decorated route again.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// A text message that will be sent to the client upon throttling.  You can include the token {n} to
        /// show this.Seconds in the message, e.g. "Wait {n} seconds before trying again".
        /// </summary>
        public string Message { get; set; }
        public void OnActionExecuted(ActionExecutedContext c)
        {
              
        }

        public void OnActionExecuting(ActionExecutingContext c)
        {
            var memCache = (IMemoryCache)c.HttpContext.RequestServices.GetService(typeof(IMemoryCache));
            var testProxy = c.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For");
            var key = 0;
            if (testProxy)
            {
                var ipAddress = IPAddress.TryParse(c.HttpContext.Request.Headers["X-Forwarded-For"], out IPAddress realClient);
                if (ipAddress)
                {
                    key = realClient.GetHashCode();
                }
            }
            else
            { 
                Message = "X-Forwarded-For not found in header"; 
                c.Result = new UnauthorizedObjectResult(Message);
            }
            if (key != 0)
            {
                key = c.HttpContext.Connection.RemoteIpAddress.GetHashCode();
            }
            memCache.TryGetValue(key, out bool forbidExecute);
            memCache.Set(key, true, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMilliseconds(1000) });
            if (forbidExecute)
            {
                if (String.IsNullOrEmpty(Message))
                    Message = $"You may only perform this action every {1000}ms.";

                c.Result = new ContentResult { Content = Message, ContentType = "text/plain" };
                // see 409 - http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
                c.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            }
            
        }
 
    }
}
