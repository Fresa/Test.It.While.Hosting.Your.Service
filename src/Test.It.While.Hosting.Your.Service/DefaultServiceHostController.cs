using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Test.It.While.Hosting.Your.Service.Delegates;

namespace Test.It.While.Hosting.Your.Service
{
    internal class DefaultServiceHostController : IServiceHostController
    {
        public DefaultServiceHostController()
        {
            ServiceController = new DefaultServiceController(this);
        }

        private readonly object _stopLock = new object();
        private event StopAsync StopPrivate = token => Task.CompletedTask;
        public event StopAsync OnStopAsync
        {
            add
            {
                lock (_stopLock)
                {
                    StopPrivate += value;
                }
            }
            remove
            {
                lock (_stopLock)
                {
                    StopPrivate -= value;
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await StopPrivate.Invoke(cancellationToken);
        }

        private readonly object _stoppedLock = new object();
        private bool _stopped;
        private CancellationToken _stoppedCancelationToken;

        private event StoppedAsync StoppedPrivate = (code, token) => 
            Task.CompletedTask;
        public event StoppedAsync OnStopped
        {
            add
            {
                lock (_stoppedLock)
                {
                    if (_stopped)
                    {
                        value.Invoke(0, _stoppedCancelationToken);
                    }
                    StoppedPrivate += value;
                }
            }
            remove
            {
                lock (_stoppedLock)
                {
                    StoppedPrivate -= value;
                }
            }
        }

        public async Task StoppedAsync(int exitCode,
            CancellationToken cancellationToken = default)
        {
            lock (_stoppedLock)
            {
                if (_stopped)
                {
                    return;
                }
                _stopped = true;
                _stoppedCancelationToken = cancellationToken;
            }

            await StoppedPrivate.Invoke(exitCode, cancellationToken);
        }

        private event StartedAsync StartedPrivate = (code, token) =>
            Task.CompletedTask;
        public event StartedAsync OnStarted
        {
            add
            {
                lock (_startedLock)
                {
                    if (_started)
                    {
                        value.Invoke(0);
                    }
                    StartedPrivate += value;
                }
            }
            remove
            {
                lock (_stoppedLock)
                {
                    StartedPrivate -= value;
                }
            }
        }

        private readonly object _startedLock = new object();
        private bool _started;

        public async Task StartedAsync(int exitCode,
            CancellationToken cancellationToken = default)
        {
            lock (_startedLock)
            {
                if (_started)
                {
                    return;
                }
                _started = true;
            }
            await StartedPrivate.Invoke(exitCode, cancellationToken);
        }

        private readonly List<(Exception, CancellationToken)> _exceptionsRaised = 
            new List<(Exception, CancellationToken)>();
        private readonly object _exceptionLock = new object();

        private event HandleExceptionAsync OnExceptionPrivate = (exception, token) => 
            Task.CompletedTask;
        public event HandleExceptionAsync OnUnhandledException
        {
            add
            {
                lock (_exceptionLock)
                {
                    foreach (var (exception, cancellationToken) in _exceptionsRaised)
                    {
                        value.Invoke(exception, cancellationToken);
                    }
                    OnExceptionPrivate += value;
                }
            }
            remove
            {
                lock (_exceptionLock)
                {
                    OnExceptionPrivate -= value;
                }
            }
        }

        public async Task RaiseExceptionAsync(Exception exception,
            CancellationToken cancellationToken)
        {
            lock (_exceptionLock)
            {
                _exceptionsRaised.Add((exception, cancellationToken));
            }

            await OnExceptionPrivate.Invoke(exception, cancellationToken);
        }

        public IServiceController ServiceController { get; }
    }
}