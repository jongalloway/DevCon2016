using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiddlewareApp
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate next;
        private readonly RequestCultureOptions options;

        public RequestCultureMiddleware(RequestDelegate next, IOptions<RequestCultureOptions> options)
        {
            this.next = next;
            this.options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            CultureInfo requestCulture = null;

            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                requestCulture = new CultureInfo(cultureQuery);
            }
            else
            {
                requestCulture = this.options.DefaultCulture;
            }

            if (requestCulture != null)
            {
#if !DNXCORE50
                Thread.CurrentThread.CurrentCulture = requestCulture;
                Thread.CurrentThread.CurrentUICulture = requestCulture;
#else
                CultureInfo.CurrentCulture = requestCulture;
                CultureInfo.CurrentUICulture = requestCulture;
#endif
            }

            return this.next(context);
        }
    }

	public static class RequestCultureMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
		{
			 return builder.UseMiddleware<RequestCultureMiddleware>();
		}
	}
}
