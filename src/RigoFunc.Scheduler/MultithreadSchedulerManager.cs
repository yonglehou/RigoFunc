// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Represents the manager which manages and schedules the tasks in multi-thread.
    /// </summary>
    public class MultithreadSchedulerManager : SchedulerManagerBase {
        private bool _IsScheduling;

        private object _SyncLock;

        private Dictionary<string, IScheduler> _Schedulers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultithreadSchedulerManager"/> class.
        /// </summary>
        public MultithreadSchedulerManager() {
            _IsScheduling = false;
            _SyncLock = new object();
            _Schedulers = new Dictionary<string, IScheduler>();
        }

        /// <summary>
        /// Creates a new scheduler with the specified name and period.
        /// </summary>
        /// <param name="schedulerName">The name of the new scheduler.</param>
        /// <param name="schedulePeriod">The schedule period of the new scheduler.</param>
        /// <returns>The new scheduler created.</returns>
        public override IScheduler CreateScheduler(string schedulerName, TimeSpan schedulePeriod) {
            IScheduler scheduler = null;

            lock (_SyncLock) {
                // check name exists. schedulers can't have same name.
                if (_Schedulers.ContainsKey(schedulerName))
                    throw new Exception("One same name scheduler had existed");

                // create new task scheduler
                var taskScheduler = new Scheduler(schedulerName, schedulePeriod, this);
                scheduler = taskScheduler;

                // store the new task scheduler
                _Schedulers.Add(schedulerName, scheduler);

                // create a new thread for this new scheduler
                var thread = new Thread(SchedulerExecHandler);
                thread.IsBackground = true;
                thread.Start(taskScheduler.Context);
            }

            return scheduler;
        }

        /// <summary>
        /// Gets the scheduler stored in the <see cref="ISchedulerManager"/>.
        /// </summary>
        /// <param name="schedulerName">The name of the scheduler to get.</param>
        /// <returns>
        /// The scheduler with the specified name retrieved.
        /// </returns>
        public override IScheduler GetScheduler(string schedulerName) {
            IScheduler scheduler = null;
            lock (_SyncLock) {
                if (_Schedulers.ContainsKey(schedulerName))
                    scheduler = _Schedulers[schedulerName];
            }

            return scheduler;
        }

        /// <summary>
        /// Starts the scheduler main thread and schedules all schedulers.
        /// </summary>
        public override void Start() {
            // change flag
            _IsScheduling = true;
        }

        /// <summary>
        /// Stops the scheduler main thread.
        /// </summary>
        public override void Stop() {
            // change flag
            _IsScheduling = false;

            // recycle all allocated schedulers
            lock (_SyncLock) {
                if (_Schedulers.Count > 0) {
                    var schedulers = new IScheduler[_Schedulers.Count];
                    _Schedulers.Values.CopyTo(schedulers, 0);
                    foreach (IScheduler scheduler in schedulers) {
                        var disposableScheduler = scheduler as IDisposable;
                        if (disposableScheduler != null)
                            disposableScheduler.Dispose();
                    }
                }

                _Schedulers.Clear();
            }
        }

        /// <summary>
        /// Recycles the specified scheduler. 
        /// This method is used internally.
        /// </summary>
        /// <param name="scheduler">The scheduler to recycle.</param>
        protected internal override void RecycleInternal(IScheduler scheduler) {
            if (scheduler == null)
                return;

            var taskScheduler = scheduler as Scheduler;
            if (taskScheduler == null)
                return;

            // remove from list
            lock (_SyncLock) {
                string schedulerName = scheduler.Name;
                if (_Schedulers.ContainsKey(schedulerName))
                    _Schedulers.Remove(schedulerName);
            }
        }

        private void SchedulerExecHandler(object parameter) {
            var context = parameter as SchedulerContext;
            if (context == null)
                return;

            var stopwatch = new Stopwatch();
            TimeSpan interval = TimeSpan.Zero;
            while (_IsScheduling) {
                // if the context is disposed, break the iteration
                if (context.IsDisposed)
                    break;

                try {
                    // check active state and run
                    if (context.SchedulerState == SchedulerState.Active) {
                        // reset
                        interval = TimeSpan.Zero;
                        if (context.SchedulePeriod > TimeSpan.Zero) {
                            stopwatch.Restart();

                            context.Run();

                            stopwatch.Stop();

                            interval = context.SchedulePeriod - stopwatch.Elapsed;
                            if (interval < TimeSpan.Zero) {
                                OnSchedulerTimeout(context.Scheduler, stopwatch.Elapsed);
                            }
                        }
                        else {
                            // this is immediate scheduler
                            context.Run();
                        }

                    }
                    else if (context.SchedulerState == SchedulerState.Inactive) {
                        // block the execution thread until receives a signal
                        context.Wait();
                    }
                }
                catch (Exception exception) {
                    OnSchedulerFailure(context.Scheduler, exception);
                }
                finally {
                    if (interval > TimeSpan.Zero) {
                        Thread.Sleep(interval);
                    }
                    else {
                        // this just for switch CPU for other works.
                        Thread.Sleep(0);
                    }
                }
            }
        }
    }
}
