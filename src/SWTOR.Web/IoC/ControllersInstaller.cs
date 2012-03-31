using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using SWTOR.Web.Controllers;

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
