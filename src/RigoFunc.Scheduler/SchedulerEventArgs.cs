// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    using System;

    /// <summary>
    /// Represents the event data for scheduler.
    /// </summary>
    public class SchedulerEventArgs : EventArgs {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerEventArgs"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="scheduleElapsed">The schedule elapsed.</param>
        internal SchedulerEventArgs(IScheduler scheduler, TimeSpan? scheduleElapsed) {
            this.Scheduler = scheduler;
            this.ScheduleElapsed = scheduleElapsed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerEventArgs"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="exception">The exception.</param>
        internal SchedulerEventArgs(IScheduler scheduler, Exception exception) {
            this.Scheduler = scheduler;
            this.Exception = exception;
        }

        /// <summary>
        /// Gets the scheduler which this context belongs to.
        /// </summary>
        /// <value>The scheduler which this context belongs to.</value>
        public IScheduler Scheduler { get; private set; }

        /// <summary>
        ///  Gets the total elapsed time measured by the Schedule process.
        /// </summary>
        /// <value>The total elapsed time.</value>
        public TimeSpan? ScheduleElapsed { get; private set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; private set; }
    }
}
