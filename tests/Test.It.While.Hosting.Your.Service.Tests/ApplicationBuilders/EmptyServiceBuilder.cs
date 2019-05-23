using System;

namespace Test.It.While.Hosting.Your.Service.Tests.ApplicationBuilders
{
    public class EmptyServiceBuilder : DefaultServiceBuilder
    {
        public override IService Create(It.Specifications.ITestConfigurer configurer)
        {
            return new EmptyService();
        }

        private class EmptyService : IService
        {
            public int Start(params string[] args)
            {
                return 0;
            }

            public int Stop()
            {
                return 0;
            }

            public event Action<Exception> OnUnhandledException;
        }
    }
}