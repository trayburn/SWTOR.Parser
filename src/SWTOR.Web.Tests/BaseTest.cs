using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Rhino.Mocks;

namespace SWTOR.Web.Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        protected IWindsorContainer Container;

        [TestInitialize]
        public void SetUp()
        {
            Container = new WindsorContainer();
            RegisterComponents(Container);
            BeforeEachTest();
        }

        protected virtual void RegisterComponents(IWindsorContainer container)
        {
        }

        protected virtual void BeforeEachTest()
        {
        }

        protected virtual void AfterEachTest()
        {
        }

        [TestCleanup]
        public void TearDown()
        {
            AfterEachTest();
            using (Container) { }
        }

        protected T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        protected T Mock<T>() 
            where T : class
        {
            return MockRepository.GenerateMock<T>();
        }

        protected void Register<T,U>() 
            where U : T
            where T : class
        {
            Container.Register(Component.For<T>().ImplementedBy<U>());
        }

        protected void Register<T>()
            where T : class
        {
            Container.Register(Component.For<T>());
        }

        protected void Register<T>(T instance)
            where T : class
        {
            Container.Register(Component.For<T>().Instance(instance));
        }

        protected T RegisterMock<T>() 
            where T : class
        {
            T mock = Mock<T>();
            Register<T>(mock);
            return mock;
        }
    }
}
