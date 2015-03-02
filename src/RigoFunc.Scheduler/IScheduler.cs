// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    using System;

    /// <summary>
    /// Provides the interfaces for the task scheduler.
    /// </summary>
    public interface IScheduler : IDisposable {
        /// <summary>
        /// Gets the name of the <see cref="IScheduler"/>.
        /// </summary>
        /// <value>The name of the <see cref="IScheduler"/>.</value>
        string Name { get; }

        /// <summary>
        /// Gets or sets the schedule period.
        /// The period is the interval between two invocations of the scheduler callback.
        /// </summary>
        /// <value>The schedule period.</value>
        TimeSpan SchedulePeriod { get; set; }

        /// <summary>
        /// Registers a new callback to this <see cref="IScheduler"/>.
        /// If the <see cref="Register<T>(Action<T> callback, T parameter)"/> had been registered, 
        /// the callback of the current method would be ignored.
        /// </summary>
        /// <param name="callback">The callback to register.</param>
        void Register(Action callback);

        /// <summary>
        /// Registers a new callback to this <see cref="IScheduler" />.
        /// If this callback had been registered, the callback registered by the
        /// <see cref="Register(Action callback)" /> method would be ignored.
        /// </summary>
        /// <param name="callback">The callback to register.</param>
        /// <param name="parameter">The parameter to be registered with the callback.</param>
        void Register(Action<object> callback, object parameter);

        /// <summary>
        /// Activates this <see cref="IScheduler"/>.
        /// </summary>
        void Activate();

        /// <summary>
        /// Inactivates this <see cref="IScheduler"/>.
        /// </summary>
        void Inactivate();

        /// <summary>
        /// Gets a value indicating whether this scheduler is active or not.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if this scheduler is active; otherwise, <see langword="false"/>.
        /// </value>
        bool IsActive { get; }
    }
}
