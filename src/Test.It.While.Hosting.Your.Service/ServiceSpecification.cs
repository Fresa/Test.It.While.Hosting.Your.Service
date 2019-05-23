using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service
{
    public abstract class ServiceSpecification<THostStarter>
        where THostStarter : class, IServiceHostStarter, new()
    {
        protected ServiceSpecification()
        {
            ServiceController = _notStartedController;
        }

        private readonly SemaphoreSlim _waitForStop = new SemaphoreSlim(0, 1);
        private readonly SemaphoreSlim _waitForStopped = new SemaphoreSlim(0, 1);
        private readonly ConcurrentBag<Exception> _exceptions = new ConcurrentBag<Exception>();

        private void RegisterException(Exception exception)
        {
            if (_exceptions.Contains(exception))
            {
                return;
            }

            if (!(exception is AggregateException aggregateException))
            {
                _exceptions.Add(exception);
                return;
            }

            foreach (var innerException in aggregateException.InnerExceptions)
            {
                _exceptions.Add(innerException);
            }
        }

        /// <summary>
        /// Execution timeout. Defaults to 3 seconds.
        /// </summary>
        protected virtual TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Application stop timeout. Defaults to 3 seconds.
        /// </summary>
        protected virtual TimeSpan StopTimeout { get; set; } = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Bootstraps and starts the hosted application.
        /// </summary>
        /// <param name="hostStarter">Service configuration</param>
        public async Task SetConfigurationAsync(THostStarter hostStarter, CancellationToken cancellationToken = default)
        {
            var controller = hostStarter.Create(new SimpleTestConfigurer(Given), StartParameters);

            ServiceController = controller.ServiceController;

            controller.OnStopped += async (exitCode, _) =>
            {
                await _notStartedController.InvokeOnStoppedAsync(exitCode, cancellationToken);
                _waitForStopped.Release();
            };

            controller.OnStopAsync += _ =>
            {
                _waitForStop.Release();
                return Task.CompletedTask;
            };
            
            controller.OnUnhandledException += (exception, _) =>
            {
                RegisterException(exception);
                _waitForStop.Release();
                _waitForStopped.Release();
                return Task.CompletedTask;
            };

            await hostStarter.StartAsync();
            HandleExceptions();

            await WhenAsync(cancellationToken);

            await WaitForStopAsync(cancellationToken);
            await WaitForStoppedAsync(cancellationToken);
        }

        private async Task WaitForStoppedAsync(CancellationToken cancellationToken)
        {
            var start = DateTime.Now;
            if (await _waitForStopped.WaitAsync(StopTimeout, cancellationToken) == false)
            {
                var stop = DateTime.Now;
                _exceptions.Add(new TimeoutException($"Waited {StopTimeout:mm\\:ss} for stopped signal. {start:HH:mm:ss.fff}->{stop:HH:mm:ss.fff}"));
            }

            HandleExceptions();
        }

        private async Task WaitForStopAsync(CancellationToken cancellationToken)
        {
            var start = DateTime.Now;
            if (await _waitForStop.WaitAsync(Timeout, cancellationToken) == false)
            {
                var stop = DateTime.Now;
                _exceptions.Add(new TimeoutException($"Waited {Timeout:mm\\:ss} for stop signal. {start:HH:mm:ss.fff}->{stop:HH:mm:ss.fff}"));
            }

            HandleExceptions();
        }

        private void HandleExceptions()
        {
            if (_exceptions.Any() == false)
            {
                return;
            }

            if (_exceptions.Count == 1)
            {
                ExceptionDispatchInfo.Capture(_exceptions.First()).Throw();
            }

            throw new AggregateException(_exceptions);
        }

        /// <summary>
        /// Defines the start parameters that will be used to bootstrap the application.
        /// </summary>
        /// <returns></returns>
        protected virtual string[] StartParameters { get; } = new string[0];

        private readonly NotStartedController _notStartedController = new NotStartedController();
        /// <summary>
        /// HostController to communicate with the hosted service application.
        /// </summary>
        protected IServiceController ServiceController { get; private set; }

        /// <summary>
        /// OBS! <see cref="ServiceController"/> is not ready here since the application is in bootstrapping phase where you control the service configuration.
        /// </summary>
        /// <param name="configurer">Service container</param>
        protected virtual void Given(IServiceContainer configurer) { }

        /// <summary>
        /// Application has started and is controlable through <see cref="ServiceController"/>.
        /// </summary>
        protected virtual Task WhenAsync(CancellationToken cancellationToken) 
            => Task.CompletedTask;
    }
}