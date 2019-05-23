using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service
{
    internal class ServiceHostingMiddleware : IMiddleware
    {
        private readonly IServiceHost _serviceHost;
        private readonly IServiceHostController _hostController;
        private Func<IDictionary<string, object>, Task> _next;

        public ServiceHostingMiddleware(IServiceHost serviceHost, IServiceHostController hostController)
        {
            _serviceHost = serviceHost;
            _hostController = hostController;
        }

        public void Initialize(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            _hostController.OnStopAsync += async cancellationToken =>
            {
                try
                {
                    var exitCode = await _serviceHost.StopAsync(cancellationToken);

                    _serviceHost.OnUnhandledException -= OnUnhandledException;

                    await _hostController.StoppedAsync(exitCode, cancellationToken);
                }
                catch (Exception exception)
                {
                    OnUnhandledException(exception);
                }
            };

            try
            {
                var startParameters = environment[Owin.StartParameters] as string[] ?? new string[0];

                _serviceHost.OnUnhandledException += OnUnhandledException;

                var startCode = await _serviceHost.StartAsync(CancellationToken.None, startParameters);
                await _hostController.StartedAsync(startCode);
            }
            catch (Exception exception)
            {
                OnUnhandledException(exception);
            }

            if (_next != null)
            {
                await _next.Invoke(environment);
            }
        }

        private void OnUnhandledException(Exception exception)
        {
            _hostController.RaiseExceptionAsync(exception);
        }
    }
}