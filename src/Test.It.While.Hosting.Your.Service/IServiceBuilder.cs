using Test.It.Specifications;
using Test.It.Starters;

namespace Test.It.While.Hosting.Your.Service
{
    /// <summary>
    /// Builds the Service host process.
    /// </summary>
    public interface IServiceBuilder
    {
        /// <summary>
        /// Use an optional service configuration
        /// </summary>
        /// <param name="configuration">Service configuration</param>
        /// <returns></returns>
        IServiceBuilder WithConfiguration(IServiceConfiguration configuration);
        
        /// <summary>
        /// Creates the Service hosting process with a test configuration.
        /// </summary>
        /// <param name="configurer">A test configuration used to override the Service configuration.</param>
        /// <returns>An application starter</returns>
        IApplicationStarter<IServiceHostController> CreateWith(ITestConfigurer configurer);
    }
}