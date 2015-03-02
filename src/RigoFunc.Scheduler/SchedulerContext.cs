// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    using System;
    using System.Threading;

    /// <summary>
    /// Represents the context of the scheduler.
    /// </summary>
    internal sealed class SchedulerContext : IDisposable {
        #region Public Interface

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerContext"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler which this context belongs to.</param>
        public SchedulerContext(IScheduler scheduler) {
            _SchedulerState = SchedulerState.Inactive;
            _Callback = null;
            _ParamCallback = null;
            _Scheduler = scheduler;
            _IsDisposed = false;
            _AutoResetEvent = new AutoResetEvent(false);
        }

        /// <summary>
        /// Gets the scheduler which this context belongs to.
        /// </summary>
        /// <value>The scheduler which this context belongs to.</value>
        public IScheduler Scheduler {
            get { return _Scheduler; }
        }

        /// <summary>
        /// Gets the schedule period. 
        /// The period is the interval between two invocations of the callback.
        /// </summary>
        /// <value>The schedule period.</value>
        public TimeSpan SchedulePeriod {
            get {
                return _Scheduler.SchedulePeriod;
            }
        }

        /// <summary>
        /// Gets the state of the scheduler.
        /// </summary>
        /// <value>The state of the scheduler.</value>
        public SchedulerState SchedulerState {
            get { return _SchedulerState; }
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="SchedulerContext"/> is disposed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if current <see cref="SchedulerContext"/> is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed {
            get { return _IsDisposed; }
        }

        /// <summary>
        /// Blocks the current thread until the current System.Threading.WaitHandle receives a signal.
        /// </summary>
        /// <returns><c>true</c> if the current instance receives a signal, <c>false</c> otherwise.</returns>
        public bool Wait() {
            return _AutoResetEvent.WaitOne();
        }

        /// <summary>
        /// Changes the scheduler state of this <see cref="SchedulerContext"/>.
        /// </summary>
        public void ChangeSchedulerState(SchedulerState state) {
            _SchedulerState = state;

            if (_SchedulerState == SchedulerState.Active) {
                _AutoResetEvent.Set();
            }
        }

        /// <summary>
        /// Invokes the callback of this <see cref="SchedulerContext"/> synchronously.
        /// </summary>
        public void Run() {
            if (_ParamCallback != null) {
                _ParamCallback(_Parameter);
            }
            else if (_Callback != null) {
                _Callback();
            }
        }

        /// <summary>
        /// Registers a new callback to this <see cref="SchedulerContext"/>.
        /// </summary>
        /// <param name="callback">The callback to register.</param>
        public void Register(Action callback) {
            // combine
            var combinedDel = Delegate.Combine(_Callback, callback);

            if (combinedDel == null)
                _Callback = null;
            else
                _Callback = (Action)combinedDel;
        }

        /// <summary>
        /// Registers a new callback to this <see cref="SchedulerContext"/>.
        /// </summary>
        /// <param name="callback">The callback to register.</param>
        /// <param name="parameter">The parameter.</param>
        public void Register(Action<object> callback, object parameter) {
            // combine
            var combinedDel = Delegate.Combine(_ParamCallback, callback);

            if (combinedDel == null)
                _ParamCallback = null;
            else
                _ParamCallback = (Action<object>)combinedDel;

            _Parameter = parameter;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            // change state to inactive
            _SchedulerState = SchedulerState.Inactive;

            // remove callback delegate invocation list
            _Callback = null;
            _ParamCallback = null;

            // set IsDisposed flag
            _IsDisposed = true;
        }

        #endregion

        #region Private Members

        private bool _IsDisposed;
        private AutoResetEvent _AutoResetEvent;
        private SchedulerState _SchedulerState;
        private IScheduler _Scheduler;
        private Action _Callback;
        private object _Parameter;
        private Action<object> _ParamCallback;

        #endregion
    }
}
