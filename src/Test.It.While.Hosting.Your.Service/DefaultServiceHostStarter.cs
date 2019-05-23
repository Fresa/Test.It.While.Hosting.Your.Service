using System.Threading;
using System.Threading.Tasks;
using Test.It.Specifications;
using Test.It.Whil.Hosting.Your.Service;

namespace Test.It.While.Hosting.Your.Service
{
    public class DefaultServiceHostStarter<TApplicationBuilder> : IServiceHostStarter
        where TApplicationBuilder : IServiceBuilder, new()
    {
        private ServiceTestServer _server;
        private ITestConfigurer _testConfigurer;
        private TApplicationBuilder _applicationBuilder;

        public IServiceHostController Create(ITestConfigurer testConfigurer, params string[] startParameters)
        {
            _applicationBuilder = new TApplicationBuilder();

            _testConfigurer = testConfigurer;
            _server = ServiceTestServer
                .Create(_applicationBuilder
                    .WithConfiguration(new DefaultServiceConfiguration(startParameters))
                    .CreateWith(_testConfigurer));
            return _server.HostController;
        }

        public void Dispose()
        {
            _server?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _server.StartAsync(cancellationToken);
        }
    }
}