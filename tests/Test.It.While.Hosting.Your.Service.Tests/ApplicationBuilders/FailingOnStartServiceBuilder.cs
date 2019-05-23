using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class FailingOnStartServiceBuilder : DefaultServiceBuilder
    {
        public override IServiceHost Create(It.Specifications.ITestConfigurer configurer)
        {
            return new FailingTestService();
        }

        private class FailingTestService : IServiceHost
        {
            public Task<int> StartAsync(CancellationToken cancellationToken = default, params string[] args)
            {
                throw new Exception("Failing to start");
            }

            public Task<int> StopAsync(CancellationToken cancellationToken = default)
            {
                return Task.FromResult(0);
            }

            public event Action<Exception> OnUnhandledException
            {
                add { }
                remove { }
            }
        }
    }
}