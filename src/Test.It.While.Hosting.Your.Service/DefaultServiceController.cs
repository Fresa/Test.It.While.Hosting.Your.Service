using System.Threading;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Service.Delegates;

namespace Test.It.While.Hosting.Your.Service
{
    internal class DefaultServiceController : IServiceController
    {
        private readonly DefaultServiceHostController _hostController;

        public DefaultServiceController(DefaultServiceHostController hostController)
        {
            _hostController = hostController;
            _hostController.OnStoppedAsync += async (code, cancellationToken) => 
                await OnStoppedAsync.Invoke(code, cancellationToken);
            _hostController.OnStopAsync += async cancellationToken => 
                await OnStopAsync.Invoke(cancellationToken);
            _hostController.OnStartedAsync += async (code, cancellationToken) =>
            {
                if (OnStartedAsync != null)
                    await OnStartedAsync.Invoke(code, cancellationToken);
            };
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await _hostController.StopAsync(cancellationToken);
        }

        public event StoppedAsync OnStoppedAsync = (_, __) => Task.CompletedTask;
        public event StartedAsync OnStartedAsync;
        public event StopAsync OnStopAsync = _ => Task.CompletedTask;
    }
}