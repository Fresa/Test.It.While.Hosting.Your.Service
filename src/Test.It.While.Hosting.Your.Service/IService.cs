using System;

namespace Test.It.While.Hosting.Your.Service
{
    public interface IService
    {
        /// <summary>
        /// Starts the Service
        /// </summary>
        /// <param name="args">Any argument that the Service might use</param>
        /// <returns>Start process status defined by the Service.</returns>
        int Start(params string[] args);

        /// <summary>
        /// Stops the Service.
        /// </summary>
        /// <returns>Exit code</returns>
        int Stop();

        /// <summary>
        /// Unhandled exception event
        /// </summary>
        event Action<Exception> OnUnhandledException;
    }
}