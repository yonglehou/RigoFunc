// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    /// <summary>
    /// Represents the state of scheduler.
    /// </summary>
    internal enum SchedulerState : byte {
        /// <summary>
        /// Indicates that the scheduler is inactive and cannot be scheduled.
        /// </summary>
        Inactive = 0,
        /// <summary>
        /// Indicates that the scheduler is active and prepared to be scheduled.
        /// </summary>
        Active = 1,
    }
}
