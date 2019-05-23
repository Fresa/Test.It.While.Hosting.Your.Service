using System;
using System.Threading;
using System.Threading.Tasks;
using Test.It.Specifications;

namespace Test.It.While.Hosting.Your.Service
{
    public interface IServiceHostStarter : IDisposable
    {
        /// <summary>
        /// Creates the hosting process.
        /// </summary>
        /// <param name="testConfigurer">Test Configurer for the application</param>
        /// <param name="startParameters">Start parameters for the application</param>
        /// <returns>Service Host Controller</returns>
        IServiceHostController Create(
            ITestConfigurer testConfigurer,
            params string[] startParameters);

        /// <summary>
        /// Starts the hosting process.
        /// </summary>
        Task StartAsync(CancellationToken cancellationToken = default);
    }
}