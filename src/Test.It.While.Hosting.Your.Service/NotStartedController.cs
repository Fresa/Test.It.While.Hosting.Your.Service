using System;
using System.Threading;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Service.Delegates;

namespace Test.It.While.Hosting.Your.Service
{
    internal class NotStartedController : IServiceController
    {
        public async Task InvokeOnStoppedAsync(int exitCode,
            CancellationToken cancellationToken = default)
        {
            await OnStopped.Invoke(exitCode, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("The server has not yet started and cannot be stopped.");
        }

        public event StoppedAsync OnStopped = (code, token) => Task.CompletedTask;
    }
}