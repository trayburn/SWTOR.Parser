using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace SWTOR.Web.Tests
{
    public abstract class BaseControllerTest<T> : BaseTest
            where T : class
    {
        protected T controller;

        protected override void RegisterComponents(IWindsorContainer container)
        {
            base.RegisterComponents(container);
            Register<T>();
        }

        protected override void BeforeEachTest()
        {
            base.BeforeEachTest();
            controller = Resolve<T>();
        }
    }
}
