using System.Collections.Generic;

namespace Test.It.While.Hosting.Your.Service
{
    internal class DefaultServiceStarter<TClient> : BaseServiceStarter<TClient>
        where TClient : IServiceHostController
    {
        public DefaultServiceStarter(IServiceHost serviceHost, IServiceConfiguration serviceConfiguration, TClient client)
        {
            Client = client;
            ServiceHost = serviceHost;
            Environment = new Dictionary<string, object> { { Service.EnvironmentKeys.StartParameters, serviceConfiguration.StartParameters } };
        }

        protected override IDictionary<string, object> Environment { get; }

        protected override TClient Client { get; }

        protected override IServiceHost ServiceHost { get; }
    }
}