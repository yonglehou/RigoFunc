// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Scheduler {
    using System;

    /// <summary>
    /// Represents the scheduler for a task.
    /// </summary>
    internal class Scheduler : IScheduler {
        private ISchedulerManager _schedulerMgr;
        private SchedulerContext  _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheduler"/> class.
        /// </summary>
        /// <param name="name">The scheduler name.</param>
        /// <param name="period">The period indicates the interval between two invocations.</param>
        /// <param name="schedulerManager">The scheduler manager.</param>
        public Scheduler(string name, TimeSpan period, ISchedulerManager schedulerManager) {
            if (name == null || name.Trim().Length == 0)
                throw new ArgumentNullException("name");
            if (schedulerManager == null)
                throw new ArgumentNullException("schedulerManager");

            SchedulePeriod = period;
            Name = name;
            _schedulerMgr = schedulerManager;

            _context = new SchedulerContext(this);
        }

        /// <summary>
        /// Gets the <see cref="Context"/> of this <see cref="Scheduler"/>.
        /// </summary>
        /// <value>The <see cref="Context"/>.</value>
        public SchedulerContext Context {
            get { return _context; }
        }

        /// <summary>
        /// Gets the name of the <see cref="Scheduler"/>.
        /// </summary>
        /// <value>The name of the <see cref="Scheduler"/>.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the schedule period. 
        /// The period is the interval between two invocations of the scheduler callback.
        /// </summary>
        /// <value>The schedule period.</value>
        public TimeSpan SchedulePeriod { get; set; }

        /// <summary>
        ///  Gets the total elapsed time measured by the Schedule process.
        /// </summary>
        /// <value>The total elapsed time.</value>
        public TimeSpan ScheduleElapsed { get; set; }

        /// <summary>
        /// Registers a new callback to this <see cref="IScheduler"/>.
        /// If the <see cref="Register(Action<object> callback, object parameter)"/> had been registered, 
        /// the callback of the current method would be ignored.
        /// </summary>
        /// <param name="callback">The callback to register.</param>
        public void Register(Action callback) {
            if (_context != null)
                _context.Register(callback);
        }

        /// <summary>
        /// Registers a new callback to this <see cref="IScheduler" />.
        /// If this callback had been registered, the callback registered by the
        /// <see cref="Register(Action callback)" /> method would be ignored.
        /// </summary>
        /// <param name="callback">The callback to register.</param>
        /// <param name="parameter">The parameter to be registered with the callback.</param>
        public void Register(Action<object> callback, object parameter) {
            if (_context != null)
                _context.Register(callback, parameter);
        }

        /// <summary>
        /// Activates this <see cref="Scheduler"/>.
        /// </summary>
        public void Activate() {
            if (_context != null)
                _context.ChangeSchedulerState(SchedulerState.Active);
        }

        /// <summary>
        /// Inactivates this <see cref="Scheduler"/>.
        /// </summary>
        public void Inactivate() {
            if (_context != null)
                _context.ChangeSchedulerState(SchedulerState.Inactive);
        }

        /// <summary>
        /// Gets a value indicating whether this scheduler is active or not.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if this scheduler is active; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsActive {
            get {
                return _context.SchedulerState == SchedulerState.Active;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if (_context != null)
                _context.Dispose();

            // tell the scheduler manager to recycle this task scheduler
            if (_schedulerMgr != null)
                _schedulerMgr.RecycleScheduler(this.Name);
        }
    }
}
