using System;
using System.Threading;
using System.Threading.Tasks;
using Test.It.Specifications;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class SlowStoppingServiceBuilder : DefaultServiceBuilder
    {
        public override IServiceHost Create(ITestConfigurer configurer)
        {
            return new SlowStoppingTestService();
        }

        private class SlowStoppingTestService : IServiceHost
        {
            public Task<int> StartAsync(CancellationToken cancellationToken = default, params string[] args)
            {
                return Task.FromResult(0);
            }

            public async Task<int> StopAsync(CancellationToken cancellationToken = default)
            {
                await Task.Delay(10000, cancellationToken);
                return 0;
            }

            public event Action<Exception> OnUnhandledException
            {
                add { }
                remove { }
            }
        }
    }
}