using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using SWTOR.Web.Controllers;
using Raven.Client.Embedded;
using Raven.Client;
using Raven.Database.Server;
using Raven.Client.Document;
using System.Configuration;

namespace SWTOR.Web.IoC
{
    public class ControllersInstaller : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(FindControllers().LifestyleTransient());
            }

            private BasedOnDescriptor FindControllers()
            {
                return AllTypes.FromThisAssembly()
                    .BasedOn<IController>()
                    .If(Component.IsInSameNamespaceAs<HomeController>())
                    .If(t => t.Name.EndsWith("Controller"));
            }
        }
}
