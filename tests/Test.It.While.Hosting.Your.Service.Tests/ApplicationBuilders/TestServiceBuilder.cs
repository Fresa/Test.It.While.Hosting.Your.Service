using System;
using System.Threading;
using System.Threading.Tasks;
using Test.It.Specifications;
using Test.It.While.Hosting.Your.Service.Tests.Applications;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class TestServiceBuilder : DefaultServiceBuilder
    {
        public override IServiceHost Create(ITestConfigurer configurer)
        {
            var app = new TestServiceApp(configurer.Configure);

            return new TestServiceHost(app);
        }

        private class TestServiceHost : IServiceHost
        {
            private readonly TestServiceApp _app;

            public TestServiceHost(TestServiceApp app)
            {
                _app = app;
            }
            
            public Task<int> StartAsync(CancellationToken cancellationToken = default, params string[] args)
            {
                try
                {
                    var startCode = _app.Start(args);
                    return Task.FromResult(startCode);
                }
                catch (Exception exception)
                {
                    OnUnhandledException?.Invoke(exception);
                    return Task.FromResult(-1);
                }
            }

            public Task<int> StopAsync(CancellationToken cancellationToken)
            {
                var stopCode = _app.Stop();
                return Task.FromResult(stopCode);
            }

            public event Action<Exception> OnUnhandledException;
        }
    }
}