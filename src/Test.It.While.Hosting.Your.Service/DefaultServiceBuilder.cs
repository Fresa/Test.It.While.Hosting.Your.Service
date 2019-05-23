using Test.It.Specifications;
using Test.It.Starters;

namespace Test.It.While.Hosting.Your.Service
{
    public abstract class DefaultServiceBuilder : IServiceBuilder
    {
        private IServiceConfiguration _configuration = new DefaultServiceConfiguration();
        public abstract IService Create(ITestConfigurer configurer);

        public IServiceBuilder WithConfiguration(IServiceConfiguration configuration)
        {
            _configuration = configuration;
            return this;
        }

        public IApplicationStarter<IServiceHostController> CreateWith(ITestConfigurer configurer)
        {            
            var application = Create(configurer);

            return new DefaultServiceStarter<IServiceHostController>(application, _configuration, new DefaultServiceHostController());
        }
    }
}