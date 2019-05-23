using System;
using System.Linq;

namespace Test.It.While.Hosting.Your.Service.Tests.Applications
{
    public class TestServiceApp
    {
        private readonly SimpleServiceContainer _serviceContainer;

        public TestServiceApp(Action<IServiceContainer> reconfigurer)
        {
            _serviceContainer = new SimpleServiceContainer();
            _serviceContainer.RegisterSingleton<IApplicationStatusReporter>(() => new ApplicationStatusReporter());

            reconfigurer(_serviceContainer);
            _serviceContainer.Verify();
        }

        public int Start(params string[] args)
        {
            if (args.Any(s => s != "start"))
            {
                throw new Exception("Missing start argument.");
            }

            var applicationStatusReporter = _serviceContainer.Resolve<IApplicationStatusReporter>();
            applicationStatusReporter.HaveStarted = true;
            return 0;
        }

        public int Stop()
        {
            _serviceContainer.Dispose();
            return 5;
        }

        public static void Main(params string[] args)
        {
            var app = new TestServiceApp(container => { });
            app.Start(args);
        }
    }
}