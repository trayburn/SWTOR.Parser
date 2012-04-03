using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using Castle.Core.Logging;

namespace SWTOR.Web.Filters
{
    public class ErrorLoggerAttribute : HandleErrorAttribute
    {
        public ErrorLoggerAttribute()
        {
            Logger = NullLogger.Instance;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext);
            base.OnException(filterContext);
        }

        public void LogError(ExceptionContext filterContext)
        {
            Logger.Error("ErrorLoggerAttribute", filterContext.Exception);
        }

        public ILogger Logger { get; set; }
    }
}