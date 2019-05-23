using System.Collections.Generic;

namespace Test.It.While.Hosting.Your.Service
{
    internal class DefaultServiceStarter<TClient> : BaseServiceStarter<TClient>
        where TClient : IServiceHostController
    {
        public DefaultServiceStarter(IService service, IServiceConfiguration serviceConfiguration, TClient client)
        {
            Client = client;
            Service = service;
            Environment = new Dictionary<string, object> { { Owin.StartParameters, serviceConfiguration.StartParameters } };
        }

        protected override IDictionary<string, object> Environment { get; }

        protected override TClient Client { get; }

        protected override IService Service { get; }
    }
}