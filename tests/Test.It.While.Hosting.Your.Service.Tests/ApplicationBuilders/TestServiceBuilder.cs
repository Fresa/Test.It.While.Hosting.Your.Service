using System;
using System.Threading.Tasks;
using Test.It.Specifications;
using Test.It.While.Hosting.Your.Service.Tests.Applications;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class TestServiceBuilder : DefaultServiceBuilder
    {
        public override IService Create(ITestConfigurer configurer)
        {
            var app = new TestServiceApp(configurer.Configure);

            return new TestServiceWrapper(app);
        }

        private class TestServiceWrapper : IService
        {
            private readonly TestServiceApp _app;

            public TestServiceWrapper(TestServiceApp app)
            {
                _app = app;
            }
            
            public int Start(params string[] args)
            {
                Task.Run(() => _app.Start(args))
                    .ContinueWith(task => OnUnhandledException?.Invoke(task.Exception), 
                        TaskContinuationOptions.OnlyOnFaulted);
                return 0;
            }

            public int Stop()
            {
                return _app.Stop();
            }

            public event Action<Exception> OnUnhandledException;
        }
    }
}