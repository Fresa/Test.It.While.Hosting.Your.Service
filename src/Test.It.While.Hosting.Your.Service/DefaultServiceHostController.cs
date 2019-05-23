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

        #region Stop
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
        #endregion

        #region Stopped
        private readonly object _stoppedLock = new object();
        private bool _stopped;
        private int _stoppedCode;
        private CancellationToken _stoppedCancelationToken;

        private event StoppedAsync StoppedPrivate = (_, __) =>
            Task.CompletedTask;
        public event StoppedAsync OnStoppedAsync
        {
            add
            {
                lock (_stoppedLock)
                {
                    if (_stopped)
                    {
                        value.Invoke(_stoppedCode, _stoppedCancelationToken);
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

        public async Task StoppedAsync(int stoppedCode,
            CancellationToken cancellationToken = default)
        {
            lock (_stoppedLock)
            {
                if (_stopped)
                {
                    return;
                }
                _stopped = true;
                _stoppedCode = stoppedCode;
                _stoppedCancelationToken = cancellationToken;
            }

            await StoppedPrivate.Invoke(stoppedCode, cancellationToken);
        }
        #endregion

        #region Started
        private event StartedAsync StartedPrivate = (_, __) =>
            Task.CompletedTask;
        public event StartedAsync OnStartedAsync
        {
            add
            {
                lock (_startedLock)
                {
                    if (_started)
                    {
                        value.Invoke(_startedCode, _startedCancelationToken);
                    }
                    StartedPrivate += value;
                }
            }
            remove
            {
                lock (_startedLock)
                {
                    StartedPrivate -= value;
                }
            }
        }

        private readonly object _startedLock = new object();
        private bool _started;
        private int _startedCode;
        private CancellationToken _startedCancelationToken;

        public async Task StartedAsync(int startedCode,
            CancellationToken cancellationToken = default)
        {
            lock (_startedLock)
            {
                if (_started)
                {
                    return;
                }
                _started = true;
                _startedCode = startedCode;
                _startedCancelationToken = cancellationToken;
            }

            await StartedPrivate.Invoke(startedCode, cancellationToken);
        }
        #endregion

        #region ExceptionRaised
        private readonly List<(Exception, CancellationToken)> _exceptionsRaised =
            new List<(Exception, CancellationToken)>();
        private readonly object _exceptionLock = new object();

        private event HandleExceptionAsync OnExceptionPrivate = (exception, token) =>
            Task.CompletedTask;
        public event HandleExceptionAsync OnUnhandledExceptionAsync
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
        #endregion

        public IServiceController ServiceController { get; }
    }
}