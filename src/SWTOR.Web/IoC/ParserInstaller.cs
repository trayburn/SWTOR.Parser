using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using SWTOR.Parser;

namespace SWTOR.Web.IoC
{
    public class ParserInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromAssemblyContaining<IParser>()
                .Pick().WithServiceAllInterfaces());
        }
    }
}
