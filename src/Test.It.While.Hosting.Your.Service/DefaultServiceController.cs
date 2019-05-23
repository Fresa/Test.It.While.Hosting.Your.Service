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
            _hostController.OnStopped += async (code, cancellationToken) => 
                await OnStopped.Invoke(code, cancellationToken);
            _hostController.OnStopAsync += async cancellationToken => 
                await OnStop.Invoke(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await _hostController.StopAsync(cancellationToken);
        }

        public event StoppedAsync OnStopped = (code, token) => Task.CompletedTask;
        public event StopAsync OnStop = token => Task.CompletedTask;
    }
}