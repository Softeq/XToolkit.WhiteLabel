using System;
using System.Reflection;

namespace Softeq.XToolkit.Common
{
    public abstract class WeakDelegate<TDelegate> where TDelegate : Delegate
    {
        protected WeakDelegate(object target, TDelegate @delegate)
        {
            Method = @delegate.GetMethodInfo();

            if (Method.IsStatic)
            {
                StaticDelegate = @delegate;

                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakDelegate's lifetime.
                    Reference = new WeakReference(target);
                }

                return;
            }

            DelegateReference = new WeakReference(@delegate.Target);
            Reference = new WeakReference(target);
        }

        /// <summary>
        ///     Gets or sets the <see cref="MethodInfo" /> corresponding to this WeakDelegate's
        ///     method passed in the constructor.
        /// </summary>
        private MethodInfo Method { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="TDelegate" /> reference to this WeakDelegate's
        ///     method passed in the constructor if method is static.
        /// </summary>
        protected TDelegate StaticDelegate { get; private set; }

        /// <summary>
        ///     Gets or sets a WeakReference to this WeakDelegate's action's target.
        ///     This is not necessarily the same as
        ///     <see cref="Reference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected WeakReference DelegateReference { get; private set; }

        /// <summary>
        ///     Gets or sets a WeakReference to the target passed when constructing
        ///     the WeakDelegate. This is not necessarily the same as
        ///     <see cref="DelegateReference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected WeakReference Reference { get; private set; }

        /// <summary>
        ///     Get a value indicating whether the WeakDelegate is static or not.
        /// </summary>
        public bool IsStatic => StaticDelegate != null;

        /// <summary>
        ///     Gets the name of the method that this WeakDelegate represents.
        /// </summary>
        public string MethodName => Method.Name;

        /// <summary>
        ///     Gets the Delegate's owner. This object is stored as a
        ///     <see cref="WeakReference" />.
        /// </summary>
        public object Target => Reference?.Target;

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
                    return Reference == null || Reference.IsAlive;
                }

                return Reference != null && Reference.IsAlive;
            }
        }

        /// <summary>
        ///     Sets the reference that this instance stores to null.
        /// </summary>
        public void MarkForDeletion()
        {
            Reference = null;
            DelegateReference = null;
            Method = null;
            StaticDelegate = null;
        }

        protected T TryExecuteWeakDelegate<T>(params object[] parameters)
        {
            var delegateTarget = DelegateReference?.Target;

            if (CanExecuteForTarget(delegateTarget))
            {
                return (T) Method.Invoke(delegateTarget, parameters);
            }

            return default;
        }

        private bool CanExecuteForTarget(object target)
        {
            return IsAlive && Method != null && target != null;
        }
    }
}
