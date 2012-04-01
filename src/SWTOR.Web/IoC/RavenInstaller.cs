using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Raven.Client;
using Raven.Client.Document;
using System.Configuration;

namespace SWTOR.Web.IoC
{
    public class RavenInstaller : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IDocumentStore dStore = null;
#if DEBUG
                // Use the Embeddable RavenDB when running locally
                var documentStore = new EmbeddableDocumentStore()
                {
                    DataDirectory = "App_Data"
                };

                NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8888);
                documentStore.UseEmbeddedHttpServer = true;
                documentStore.Configuration.Port = 8888;
                dStore = documentStore;
#endif
#if !DEBUG
            var documentStore = new DocumentStore()
            {
                 Url = ConfigurationManager.AppSettings["RAVENHQ_CONNECTION_STRING"]
            };
            dStore = documentStore;
#endif

            dStore.Initialize();

            container.Register(
                Component.For<IDocumentStore>()
                    .Instance(dStore).LifeStyle.Singleton,
                Component.For<IDocumentSession>()
                    .UsingFactoryMethod(k => k.Resolve<IDocumentStore>().OpenSession())
                    .LifestylePerWebRequest(),
                AllTypes.FromThisAssembly().Where(m => m.Name.EndsWith("Repo"))
                    .WithServiceAllInterfaces().LifestylePerWebRequest()
            );
        }
    }
}
