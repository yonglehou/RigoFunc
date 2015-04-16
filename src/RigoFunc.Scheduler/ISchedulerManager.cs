// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    using System;

    /// <summary>
    /// Provides the interfaces for manages all the schedulers derived from <see cref="IScheduler"/>.
    /// </summary>
    public interface ISchedulerManager : IDisposable {
        /// <summary>
        /// Occurs when the scheduler dispatching is timeout.
        /// </summary>
        event EventHandler<SchedulerEventArgs> SchedulerTimeOut;

        /// <summary>
        /// Occurs when the failure of a scheduler happens while scheduling.
        /// </summary>
        event EventHandler<SchedulerEventArgs> SchedulerFailure;

        /// <summary>
        /// Creates a new scheduler with the specified name and period.
        /// </summary>
        /// <param name="schedulerName">The name of the new scheduler.</param>
        /// <param name="schedulePeriod">The schedule period of the new scheduler.</param>
        /// <returns>
        /// The new scheduler created.
        /// </returns>
        IScheduler CreateScheduler(string schedulerName, TimeSpan schedulePeriod);

        /// <summary>
        /// Gets the scheduler stored in the <see cref="ISchedulerManager"/>.
        /// </summary>
        /// <param name="schedulerName">The name of the scheduler to get.</param>
        /// <returns>The scheduler with the specified name retrieved.</returns>
        IScheduler GetScheduler(string schedulerName);

        /// <summary>
        /// Recycles the scheduler by the specified name.
        /// </summary>
        /// <param name="schedulerName">The name of the scheduler to recycle.</param>
        void RecycleScheduler(string schedulerName);
    }
}
