using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.It.Starters;
using Test.It.While.Hosting.Your.Service;

namespace Test.It.Whil.Hosting.Your.Service
{
    internal class ServiceTestServer : IDisposable
    {
        private readonly DefaultApplicationBuilder _appBuilder;
        private readonly IDictionary<string, object> _environment;

        private ServiceTestServer(IApplicationStarter<IServiceHostController> applicationStarter)
        {
            _appBuilder = new DefaultApplicationBuilder();
            var builder = new ControllerProvidingAppBuilder<IServiceHostController>(_appBuilder);
            _environment = applicationStarter.Start(builder);
            HostController = builder.Controller;
        }

        public IServiceHostController HostController { get; }

        public static ServiceTestServer Create(IApplicationStarter<IServiceHostController> applicationStarter)
        {
            return new ServiceTestServer(applicationStarter);
        }

        public async Task StartAsync()
        {
            await _appBuilder.Build()(_environment);
        }

        public void Dispose()
        {
        }
    }
}