using System;
using System.Threading;
using Test.It.Specifications;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class SlowStoppingServiceBuilder : DefaultServiceBuilder
    {
        public override IService Create(ITestConfigurer configurer)
        {
            return new SlowStoppingTestService();
        }

        private class SlowStoppingTestService : IService
        {
            public int Start(params string[] args)
            {
                return 0;
            }

            public int Stop()
            {
                Thread.Sleep(10000);
                return 0;
            }

            public event Action<Exception> OnUnhandledException;
        }
    }
}