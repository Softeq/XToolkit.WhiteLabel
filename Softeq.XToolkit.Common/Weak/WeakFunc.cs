// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;

namespace Softeq.XToolkit.Common.Weak
{
    /// <summary>
    ///     Stores a Func&lt;T&gt; without causing a hard reference
    ///     to be created to the Func's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="TResult">
    ///     The type of the result of the Func that will be stored
    ///     by this weak reference.
    /// </typeparam>
    public sealed class WeakFunc<TResult> : WeakDelegate<Func<TResult>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc{TResult}"/> class.
        /// </summary>
        /// <param name="func">The Func that will be associated to this instance.</param>
        public WeakFunc(Func<TResult> func)
            : this(func.Target, func)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc{TResult}"/> class.
        /// </summary>
        /// <param name="target">The Func's owner.</param>
        /// <param name="func">The Func that will be associated to this instance.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "Method should fail with an exception if func is null.")]
        public WeakFunc(object? target, Func<TResult> func)
            : base(target, func)
        {
        }

        /// <summary>
        ///     Executes the action. This only happens if the Func's owner
        ///     is still alive.
        /// </summary>
        /// <returns>The result of the Func stored as reference.</returns>
        public TResult Execute()
        {
            if (StaticDelegate != null)
            {
                return IsCustomTargetAlive
                    ? StaticDelegate.Invoke()
                    : default!;
            }

            return TryExecuteWeakDelegate<TResult>();
        }
    }

    /// <summary>
    ///     Stores an Func without causing a hard reference
    ///     to be created to the Func's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Func's parameter.</typeparam>
    /// <typeparam name="TResult">The type of the Func's return value.</typeparam>
    public sealed class WeakFunc<T, TResult> : WeakDelegate<Func<T, TResult>>, IExecuteWithObjectAndResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc{T, TResult}"/> class.
        /// </summary>
        /// <param name="func">The Func that will be associated to this instance.</param>
        public WeakFunc(Func<T, TResult> func)
            : this(func.Target, func)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc{T, TResult}"/> class.
        /// </summary>
        /// <param name="target">The Func's owner.</param>
        /// <param name="func">The Func that will be associated to this instance.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "Method should fail with an exception if func is null.")]
        public WeakFunc(object? target, Func<T, TResult> func)
            : base(target, func)
        {
        }

        /// <summary>
        ///     Executes the Func with a parameter of type object. This parameter
        ///     will be casted to T. This method implements <see cref="IExecuteWithObject.ExecuteWithObject" />
        ///     and can be useful if you store multiple WeakFunc{T} instances but don't know in advance
        ///     what type T represents.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter that will be passed to the Func after
        ///     being casted to T.
        /// </param>
        /// <returns>The result of the execution as object, to be casted to T.</returns>
        public object? ExecuteWithObject(object parameter)
        {
            var parameterCasted = (T) parameter;
            return Execute(parameterCasted);
        }

        /// <summary>
        ///     Executes the Func. This only happens if the Func's owner
        ///     is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        /// <returns>The result of the Func stored as reference.</returns>
        public TResult Execute(T parameter)
        {
            if (StaticDelegate != null)
            {
                return IsCustomTargetAlive
                    ? StaticDelegate.Invoke(parameter)
                    : default!;
            }

            return TryExecuteWeakDelegate<TResult>(parameter);
        }
    }
}
