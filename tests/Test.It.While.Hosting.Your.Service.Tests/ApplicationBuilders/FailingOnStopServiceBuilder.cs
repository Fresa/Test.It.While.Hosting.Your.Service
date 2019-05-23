using System;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class FailingOnStopServiceBuilder : DefaultServiceBuilder
    {
        public override IService Create(It.Specifications.ITestConfigurer configurer)
        {
            return new FailingTestService();
        }

        private class FailingTestService : IService
        {
            public int Start(params string[] args)
            {
                return 0;
            }

            public int Stop()
            {
                throw new Exception("Failing to stop");
            }

            public event Action<Exception> OnUnhandledException;
        }
    }
}