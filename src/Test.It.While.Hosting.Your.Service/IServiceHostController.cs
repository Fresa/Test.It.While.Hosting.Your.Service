using System;
using System.Threading;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Service.Delegates;

namespace Test.It.While.Hosting.Your.Service
{
    /// <summary>
    /// Defines the communication channels between the hosted Service and the Test Framework.
    /// </summary>
    public interface IServiceHostController
    {
        /// <summary>
        /// Triggered when the service is requested to stop
        /// </summary>
        event StopAsync OnStopAsync;

        /// <summary>
        /// Stop the service
        /// </summary>
        Task StopAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Triggered when the service has stopped
        /// </summary>
        event StoppedAsync OnStopped;

        /// <summary>
        /// Signal service started
        /// </summary>
        /// <param name="startCode">The code the service reported after starting</param>
        /// <param name="cancellationToken"></param>
        Task StartedAsync(int startCode, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Triggered when the service has started
        /// </summary>
        event StartedHandler OnStarted;

        /// <summary>
        /// Signal service stopped
        /// </summary>
        /// <param name="exitCode">The code the service reported when stopped</param>
        /// <param name="cancellationToken"></param>
        Task StoppedAsync(int exitCode, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Triggered when an unhandled exception is raised.
        /// </summary>
        event HandleExceptionAsync OnUnhandledException;

        /// <summary>
        /// Raises an exception. 
        /// </summary>
        /// <param name="exception">Exception raised by the application</param>
        /// <param name="cancellationToken"></param>
        Task RaiseExceptionAsync(Exception exception,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Controller for communicating with the service. Usually exposed to the test instance.
        /// </summary>
        IServiceController ServiceController { get; }
    }
}