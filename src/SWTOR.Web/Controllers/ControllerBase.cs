using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Logging;

namespace SWTOR.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public ControllerBase()
        {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; private set; }
    }
}
