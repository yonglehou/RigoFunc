// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    using System;

    /// <summary>
    /// Provides the generic and basic interfaces for all scheduler managers.
    /// </summary>
    public abstract class SchedulerManagerBase : ISchedulerManager {
        /// <summary>
        /// Occurs when the scheduler dispatching is timeout.
        /// </summary>
        public event EventHandler<SchedulerEventArgs> SchedulerTimeOut;

        /// <summary>
        /// Occurs when the failure of a scheduler happens while scheduling.
        /// </summary>
        public event EventHandler<SchedulerEventArgs> SchedulerFailure;
        
        /// <summary>
        /// Creates a new scheduler with the specified name and period.
        /// </summary>
        /// <param name="schedulerName">The name of the new scheduler.</param>
        /// <param name="schedulePeriod">The schedule period of the new scheduler.</param>
        /// <returns>
        /// The new scheduler created.
        /// </returns>
        public abstract IScheduler CreateScheduler(string schedulerName, TimeSpan schedulePeriod);

        /// <summary>
        /// Gets the scheduler stored in the <see cref="ISchedulerManager"/>.
        /// </summary>
        /// <param name="schedulerName">The name of the scheduler to get.</param>
        /// <returns>
        /// The scheduler with the specified name retrieved.
        /// </returns>
        public abstract IScheduler GetScheduler(string schedulerName);

        /// <summary>
        /// Recycles the scheduler by the specified name.
        /// </summary>
        /// <param name="schedulerName">The name of the scheduler to recycle.</param>
        public virtual void RecycleScheduler(string schedulerName) {
            // get scheduler
            IScheduler scheduler = GetScheduler(schedulerName);
            if (scheduler == null)
                return;

            // dispose
            if (scheduler != null)
                scheduler.Dispose();

            // recycle
            RecycleInternal(scheduler);
        }

        /// <summary>
        /// Starts the scheduler dispatch thread and schedules all schedulers.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops the scheduler dispatch thread.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Recycles the specified scheduler. This method is for internal usage only.
        /// </summary>
        /// <param name="scheduler">The scheduler to recycle.</param>
        protected internal abstract void RecycleInternal(IScheduler scheduler);

        /// <summary>
        /// Raises the <see cref="SchedulerTimeOut"/> event.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="scheduleElapsed">The schedule elapsed.</param>
        protected virtual void OnSchedulerTimeout(IScheduler scheduler, TimeSpan? scheduleElapsed) {
            var handler = SchedulerTimeOut;
            if (handler != null) {
                handler(this, new SchedulerEventArgs(scheduler, scheduleElapsed));
            }
        }

        /// <summary>
        /// Raises the <see cref="SchedulerFailure" /> event.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="exception">The exception.</param>
        protected virtual void OnSchedulerFailure(IScheduler scheduler, Exception exception) {
            var handler = SchedulerFailure;
            if (handler != null) {
                handler(this, new SchedulerEventArgs(scheduler, exception));
            }
        }
    }
}
