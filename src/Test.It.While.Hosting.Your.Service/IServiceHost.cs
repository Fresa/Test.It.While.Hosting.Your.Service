using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service
{
    public interface IServiceHost
    {
        /// <summary>
        /// Starts the Service
        /// </summary>
        /// <param name="cancellationToken">Cancels the start process</param>
        /// <param name="args">Any argument that the Service might use</param>
        /// <returns>Start process status defined by the Service.</returns>
        Task<int> StartAsync(CancellationToken cancellationToken = default, params string[] args);

        /// <summary>
        /// Stops the Service.
        /// </summary>
        /// <param name="cancellationToken">Cancels the stop process</param>
        /// <returns>Exit code</returns>
        Task<int> StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Unhandled exception event
        /// </summary>
        event Action<Exception> OnUnhandledException;
    }
}