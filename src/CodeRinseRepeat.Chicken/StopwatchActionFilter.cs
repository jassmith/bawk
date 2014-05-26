using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CodeRinseRepeat.Chicken
{
    public class StopwatchAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch stopwatch;

        public StopwatchAttribute ()
        {
            stopwatch = new Stopwatch ();
        }

        public override void OnActionExecuting (HttpActionContext actionContext)
        {
            stopwatch.Start ();
        }

        public override void OnActionExecuted (HttpActionExecutedContext actionExecutedContext)
        {
            stopwatch.Stop ();
            actionExecutedContext.Response.Headers.Add ("X-Stopwatch", stopwatch.Elapsed.ToString ());
            actionExecutedContext.Response.Headers.Add ("X-Stopwatch-Milliseconds", stopwatch.Elapsed.TotalMilliseconds.ToString (CultureInfo.InvariantCulture));
        }
    }
}