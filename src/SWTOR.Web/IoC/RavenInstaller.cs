using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Raven.Client;
using Raven.Client.Document;
using System.Configuration;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace SWTOR.Web.IoC
{
    public class RavenInstaller : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IDocumentStore dStore = null;
#if DEBUG
            // Use the Embeddable RavenDB when running locally
            container.Register(
                Component.For<IDocumentStore>().UsingFactoryMethod((k, c) =>
                {
                    var docStore = new EmbeddableDocumentStore()
                    {
                        DataDirectory = "App_Data"
                    };
                    NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8888);
                    docStore.UseEmbeddedHttpServer = true;
                    docStore.Configuration.Port = 8888;
                    return docStore;
                }).LifeStyle.Singleton
                );
#endif
#if !DEBUG
            container.Register(
                Component.For<IDocumentStore>().UsingFactoryMethod((k, c) =>
                {
                    return new DocumentStore()
                    {
                        Url = ConfigurationManager.AppSettings["RAVENHQ_CONNECTION_STRING"]
                    }.Initialize();
                }).LifeStyle.Singleton
                );
#endif

            container.Register(
                Component.For<IDocumentSession>()
                    .UsingFactoryMethod(k => k.Resolve<IDocumentStore>().OpenSession())
                    .LifestylePerWebRequest(),
                AllTypes.FromThisAssembly().Where(m => m.Name.EndsWith("Repo"))
                    .WithServiceAllInterfaces().LifestylePerWebRequest()
            );
        }
    }
}
