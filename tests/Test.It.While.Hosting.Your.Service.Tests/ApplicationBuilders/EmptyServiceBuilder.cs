using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class EmptyServiceBuilder : DefaultServiceBuilder
    {
        public override IServiceHost Create(It.Specifications.ITestConfigurer configurer)
        {
            return new EmptyService();
        }

        private class EmptyService : IServiceHost
        {
            public Task<int> StartAsync(CancellationToken cancellationToken = default, params string[] args)
            {
                return Task.FromResult(0);
            }

            public Task<int> StopAsync(CancellationToken cancellationToken = default)
            {
                return Task.FromResult(0);
            }

            public event Action<Exception> OnUnhandledException;
        }
    }
}