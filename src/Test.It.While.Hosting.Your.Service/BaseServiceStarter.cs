using System.Collections.Generic;
using Test.It.ApplicationBuilders;
using Test.It.Starters;

namespace Test.It.While.Hosting.Your.Service
{
    public abstract class BaseServiceStarter<TClient> : IApplicationStarter<TClient>
        where TClient : IServiceHostController
    {
        protected abstract TClient Client { get; }

        protected abstract IServiceHost ServiceHost { get; }

        protected abstract IDictionary<string, object> Environment { get; }

        public virtual IDictionary<string, object> Start(IApplicationBuilder<TClient> applicationBuilder)
        {
            applicationBuilder.WithController(Client).Use(new ServiceHostingMiddleware(ServiceHost, Client));
            return Environment;
        }
    }
}