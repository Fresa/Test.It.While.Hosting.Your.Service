using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service
{
    internal class ServiceHostingMiddleware : IMiddleware
    {
        private readonly IService _service;
        private readonly IServiceHostController _hostController;
        private Func<IDictionary<string, object>, Task> _next;

        public ServiceHostingMiddleware(IService service, IServiceHostController hostController)
        {
            _service = service;
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
                    await Task.Run(() =>
                    {
                        var exitCode = _service.Stop();

                        _service.OnUnhandledException -= OnUnhandledException;
                        _hostController.StoppedAsync(exitCode, cancellationToken);
                    }, cancellationToken);
                }
                catch (Exception exception)
                {
                    OnUnhandledException(exception);
                }
            };

            try
            {
                await Task.Run(() =>
                {
                    var startParameters = environment[Owin.StartParameters] as string[] ?? new string[0];

                    _service.OnUnhandledException += OnUnhandledException;

                    var startCode = _service.Start(startParameters);
                    _hostController.StartedAsync(startCode);
                });
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