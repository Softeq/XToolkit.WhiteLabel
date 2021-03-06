﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Timers
{
    /// <summary>
    ///     Represents methods and properties to execute an action in a time interval.
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        ///     Gets a value indicating whether the timer should run action.
        /// </summary>
        /// <value><c>true</c> if Task should run; otherwise, <c>false</c>.</value>
        bool IsActive { get; }

        /// <summary>
        ///     Starts running task by setting Enabled to true.
        /// </summary>
        void Start();

        /// <summary>
        ///     Stops running task by setting Enabled to false.
        /// </summary>
        void Stop();
    }
}
