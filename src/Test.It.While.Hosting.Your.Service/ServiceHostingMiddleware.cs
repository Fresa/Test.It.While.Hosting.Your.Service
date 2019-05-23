﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Test.It.While.Hosting.Your.Service
{
    internal class ServiceHostingMiddleware : IMiddleware
    {
        private readonly IServiceHost _serviceHost;
        private readonly IServiceHostController _serviceHostController;
        private Func<IDictionary<string, object>, Task> _next;

        public ServiceHostingMiddleware(IServiceHost serviceHost, IServiceHostController serviceHostController)
        {
            _serviceHost = serviceHost;
            _serviceHostController = serviceHostController;
        }

        public void Initialize(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            _serviceHostController.OnStopAsync += async cancellationToken =>
            {
                try
                {
                    var exitCode = await _serviceHost.StopAsync(cancellationToken);

                    _serviceHost.OnUnhandledException -= OnUnhandledException;

                    await _serviceHostController.StoppedAsync(exitCode, cancellationToken);
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
                await _serviceHostController.StartedAsync(startCode);
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
            _serviceHostController.RaiseExceptionAsync(exception);
        }
    }
}