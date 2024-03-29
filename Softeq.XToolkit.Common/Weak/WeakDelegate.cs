﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Softeq.XToolkit.Common.Weak
{
    public abstract class WeakDelegate<TDelegate> where TDelegate : Delegate
    {
        protected WeakDelegate(object? target, TDelegate @delegate)
        {
            Method = @delegate.GetMethodInfo();

            if (Method.IsStatic)
            {
                StaticDelegate = @delegate;

                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakDelegate's lifetime.
                    CustomTargetReference = new WeakReference(target);
                }

                return;
            }

            AssertCompilerGeneratedTarget(@delegate);
            DelegateTargetReference = new WeakReference(@delegate.Target);
            if (!ReferenceEquals(@delegate.Target, target))
            {
                CustomTargetReference = new WeakReference(target);
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="MethodInfo" /> corresponding to this WeakDelegate's
        ///     method passed in the constructor.
        /// </summary>
        private MethodInfo? Method { get; set; }

        /// <summary>
        ///     Gets the <typeparamref name="TDelegate"/> reference to this WeakDelegate's
        ///     method passed in the constructor if method is static.
        /// </summary>
        protected TDelegate? StaticDelegate { get; private set; }

        /// <summary>
        ///     Gets or sets a WeakReference to this WeakDelegate's action's target.
        ///     This is not necessarily the same as
        ///     <see cref="CustomTargetReference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        private WeakReference? DelegateTargetReference { get; set; }

        /// <summary>
        ///     Gets or sets a WeakReference to the target passed when constructing
        ///     the WeakDelegate. This is not necessarily the same as
        ///     <see cref="DelegateTargetReference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        private WeakReference? CustomTargetReference { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the WeakDelegate is static or not.
        /// </summary>
        public bool IsStatic => StaticDelegate != null;

        /// <summary>
        ///     Gets the name of the method that this WeakDelegate represents.
        /// </summary>
        public string? MethodName => Method?.Name;

        /// <summary>
        ///     Gets the Delegate's owner. This object is stored as a
        ///     <see cref="T:System.WeakReference" />.
        /// </summary>
        public object? Target => CustomTargetReference != null
            ? CustomTargetReference.Target
            : DelegateTargetReference?.Target;

        protected bool IsCustomTargetAlive => CustomTargetReference == null || CustomTargetReference.IsAlive;

        /// <summary>
        ///     Gets a value indicating whether the Delegate's owner is still alive, or if it was collected
        ///     by the Garbage Collector already.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                if (IsStatic)
                {
                    return IsCustomTargetAlive && StaticDelegate != null;
                }

                return IsCustomTargetAlive
                       && DelegateTargetReference != null
                       && DelegateTargetReference.IsAlive;
            }
        }

        /// <summary>
        ///     Sets the reference that this instance stores to null.
        /// </summary>
        public void MarkForDeletion()
        {
            CustomTargetReference = null;
            DelegateTargetReference = null;
            Method = null;
            StaticDelegate = null;
        }

        protected T TryExecuteWeakDelegate<T>(params object?[] parameters)
        {
            var delegateTarget = GetExecutionTarget();

            if (delegateTarget != null && Method != null)
            {
                try
                {
                    return (T) Method.Invoke(delegateTarget, parameters);
                }
                catch (TargetInvocationException e) when (e.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                }
            }

            return default!;
        }

        private object? GetExecutionTarget()
        {
            return IsCustomTargetAlive
                ? DelegateTargetReference?.Target
                : null;
        }

        [Conditional("DEBUG")]
        private static void AssertCompilerGeneratedTarget(TDelegate @delegate)
        {
            var isCompilerGenerated = @delegate.Target.GetType().GetCustomAttribute<CompilerGeneratedAttribute>() != null;
            if (!isCompilerGenerated)
            {
                return;
            }

            Debug.WriteLine("WeakDelegate's target is compiler-generated, it might be garbage-collected");
            Debug.WriteLine(Environment.StackTrace);
        }
    }
}
