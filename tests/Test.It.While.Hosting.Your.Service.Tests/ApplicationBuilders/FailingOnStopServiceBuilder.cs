using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class FailingOnStopServiceBuilder : DefaultServiceBuilder
    {
        public override IServiceHost Create(It.Specifications.ITestConfigurer configurer)
        {
            return new FailingTestService();
        }

        private class FailingTestService : IServiceHost
        {
            public Task<int> StartAsync(CancellationToken cancellationToken = default, params string[] args)
            {
                return Task.FromResult(0);
            }

            public Task<int> StopAsync(CancellationToken cancellationToken = default)
            {
                throw new Exception("Failing to stop");
            }

            public event Action<Exception> OnUnhandledException;
        }
    }
}