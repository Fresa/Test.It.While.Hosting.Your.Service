using System;
using System.Threading;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Service.Delegates;

namespace Test.It.While.Hosting.Your.Service
{
    internal class NotStartedController : IServiceController
    {
        public async Task InvokeOnStoppedAsync(int stoppedCode,
            CancellationToken cancellationToken = default)
        {
            await OnStoppedAsync.Invoke(stoppedCode, cancellationToken);
        }

        public async Task InvokeOnStartedAsync(int startedCode,
            CancellationToken cancellationToken = default)
        {
            await OnStartedAsync.Invoke(startedCode, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("The server has not yet started and cannot be stopped.");
        }

        public event StoppedAsync OnStoppedAsync = (code, token) => Task.CompletedTask;
        public event StartedAsync OnStartedAsync = (code, token) => Task.CompletedTask;
    }
}