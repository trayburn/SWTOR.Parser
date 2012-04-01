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
using SWTOR.Parser;
using SWTOR.Web.Data;

namespace SWTOR.Web.IoC
{
    public class RavenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
#if DEBUG
            // Use the Embeddable RavenDB when running locally
            container.Register(
                Component.For<IDocumentStore>().UsingFactoryMethod((k, c) =>
                {
                    var docStore = new EmbeddableDocumentStore()
                    {
                        DataDirectory = "App_Data",
                        Conventions =
                        {
                            DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites
                        }
                    };
                    NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8888);
                    docStore.UseEmbeddedHttpServer = true;
                    docStore.Configuration.Port = 8888;
                    docStore.Initialize();
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
                        ConnectionStringName = "RavenDB",
                        Conventions =
                        {
                            DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites
                        }
                    }.Initialize();
                }).LifeStyle.Singleton
                );
#endif

            container.Register(
                Component.For<IDocumentSession>()
                    .UsingFactoryMethod(k => k.Resolve<IDocumentStore>().OpenSession())
                    .LifestylePerWebRequest(),
                Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>))
            );
        }
    }
}
