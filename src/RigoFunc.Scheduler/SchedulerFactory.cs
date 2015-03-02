// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    using System;

    /// <summary>
    /// Class SchedulerFactory.
    /// </summary>
    public class SchedulerFactory {
        /// <summary>
        /// Creates the scheduler.
        /// </summary>
        /// <param name="schedulerName">The name of the scheduler.</param>
        /// <param name="schedulePeriod">The schedule period.</param>
        /// <param name="schedulerMgr">The scheduler manager.</param>
        /// <returns>
        /// The new scheduler created.
        /// </returns>
        public virtual IScheduler CreateScheduler(string schedulerName, TimeSpan schedulePeriod, ISchedulerManager schedulerMgr) {
            return new Scheduler(schedulerName, schedulePeriod, schedulerMgr);
        }
    }
}
