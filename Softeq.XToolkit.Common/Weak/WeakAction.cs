﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;

namespace Softeq.XToolkit.Common.Weak
{
    public sealed class WeakAction : WeakDelegate<Action>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WeakAction" /> class.
        /// </summary>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(Action action)
            : this(action.Target, action)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeakAction" /> class.
        /// </summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="action">The action that will be associated to this instance.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "Method should fail with an exception if action is null.")]
        public WeakAction(object? target, Action action)
            : base(target, action)
        {
        }

        /// <summary>
        ///     Executes the action. This only happens if the action's owner
        ///     is still alive.
        /// </summary>
        public void Execute()
        {
            if (StaticDelegate != null)
            {
                if (IsCustomTargetAlive)
                {
                    StaticDelegate.Invoke();
                }

                return;
            }

            TryExecuteWeakDelegate<object>();
        }
    }

    /// <summary>
    ///     Stores an Action without causing a hard reference
    ///     to be created to the Action's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Action's parameter.</typeparam>
    ////[ClassInfo(typeof(WeakAction))]
    public sealed class WeakAction<T> : WeakDelegate<Action<T>>, IExecuteWithObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakAction{T}"/> class.
        /// </summary>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(Action<T> action)
            : this(action.Target, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakAction{T}"/> class.
        /// </summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="action">The action that will be associated to this instance.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "Method should fail with an exception if action is null.")]
        public WeakAction(object? target, Action<T> action)
            : base(target, action)
        {
        }

        /// <summary>
        ///     Executes the action with a parameter of type object. This parameter
        ///     will be casted to T. This method implements <see cref="IExecuteWithObject.ExecuteWithObject" />
        ///     and can be useful if you store multiple WeakAction{T} instances but don't know in advance
        ///     what type T represents.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter that will be passed to the action after
        ///     being casted to T.
        /// </param>
        public void ExecuteWithObject(object parameter)
        {
            var parameterCasted = (T) parameter;
            Execute(parameterCasted);
        }

        /// <summary>
        ///     Executes the action. This only happens if the action's owner
        ///     is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        public void Execute(T parameter)
        {
            if (StaticDelegate != null)
            {
                if (IsCustomTargetAlive)
                {
                    StaticDelegate.Invoke(parameter);
                }

                return;
            }

            TryExecuteWeakDelegate<object>(parameter);
        }
    }
}
