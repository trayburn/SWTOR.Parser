using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Facilities.Logging;
using SWTOR.Web.Filters;

namespace SWTOR.Web.IoC
{
    public class LoggingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<LoggingFacility>(c => c.UseLog4Net().WithAppConfig());
            container.Register(Component.For<ErrorLoggerAttribute>());
        }
    }
}
