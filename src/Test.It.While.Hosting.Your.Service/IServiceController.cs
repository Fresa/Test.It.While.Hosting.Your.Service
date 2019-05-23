using System.Threading;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Service.Delegates;

namespace Test.It.While.Hosting.Your.Service
{
    public interface IServiceController
    {
        /// <summary>
        /// Sends a stop command to the service.
        /// </summary>
        Task StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Triggered when the service has stopped.
        /// </summary>
        event StoppedAsync OnStoppedAsync;

        /// <summary>
        /// Triggered when the service has started.
        /// </summary>
        event StartedAsync OnStartedAsync;
    }
}