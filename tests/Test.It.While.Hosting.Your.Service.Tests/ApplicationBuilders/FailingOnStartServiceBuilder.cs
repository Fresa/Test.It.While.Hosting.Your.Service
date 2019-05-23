using System;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class FailingOnStartServiceBuilder : DefaultServiceBuilder
    {
        public override IService Create(It.Specifications.ITestConfigurer configurer)
        {
            return new FailingTestService();
        }

        private class FailingTestService : IService
        {
            public int Start(params string[] args)
            {
                throw new Exception("Failing to start");
            }

            public int Stop()
            {
                return 0;
            }

            public event Action<Exception> OnUnhandledException;
        }
    }
}