// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Softeq.XToolkit.Common
{
    public class WeakAction
    {
        private Action _staticAction;

        /// <summary>
        ///     Initializes an empty instance of the <see cref="WeakAction" /> class.
        /// </summary>
        protected WeakAction()
        {
        }

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
        public WeakAction(object target, Action action)
        {
            if (action.GetMethodInfo().IsStatic)
            {
                _staticAction = action;

                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakAction's lifetime.
                    Reference = new WeakReference(target);
                }

                return;
            }

            Method = action.GetMethodInfo();
            ActionReference = new WeakReference(action.Target);
            Reference = new WeakReference(target);
        }

        /// <summary>
        ///     Gets or sets the <see cref="MethodInfo" /> corresponding to this WeakAction's
        ///     method passed in the constructor.
        /// </summary>
        protected MethodInfo Method { get; set; }

        /// <summary>
        ///     Gets the name of the method that this WeakAction represents.
        /// </summary>
        public virtual string MethodName => _staticAction != null ? _staticAction.GetMethodInfo().Name : Method.Name;

        /// <summary>
        ///     Gets or sets a WeakReference to this WeakAction's action's target.
        ///     This is not necessarily the same as
        ///     <see cref="Reference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected WeakReference ActionReference { get; set; }

        /// <summary>
        ///     Gets or sets a WeakReference to the target passed when constructing
        ///     the WeakAction. This is not necessarily the same as
        ///     <see cref="ActionReference" />, for example if the
        ///     method is anonymous.
        /// </summary>
        protected WeakReference Reference { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the WeakAction is static or not.
        /// </summary>
        public bool IsStatic => _staticAction != null;

        /// <summary>
        ///     Gets a value indicating whether the Action's owner is still alive, or if it was collected
        ///     by the Garbage Collector already.
        /// </summary>
        public virtual bool IsAlive
        {
            get
            {
                if (_staticAction == null
                    && Reference == null)
                {
                    return false;
                }

                if (_staticAction != null)
                {
                    if (Reference != null)
                    {
                        return Reference.IsAlive;
                    }

                    return true;
                }

                return Reference.IsAlive;
            }
        }

        /// <summary>
        ///     Gets the Action's owner. This object is stored as a
        ///     <see cref="WeakReference" />.
        /// </summary>
        public object Target => Reference?.Target;

        /// <summary>
        ///     The target of the weak reference.
        /// </summary>
        protected object ActionTarget => ActionReference?.Target;

        /// <summary>
        ///     Executes the action. This only happens if the action's owner
        ///     is still alive.
        /// </summary>
        public void Execute()
        {
            if (_staticAction != null)
            {
                _staticAction();
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null
                    && ActionReference != null
                    && actionTarget != null)
                {
                    Method.Invoke(actionTarget, null);

                    // ReSharper disable RedundantJumpStatement
                    return;
                    // ReSharper restore RedundantJumpStatement
                }
            }
        }

        /// <summary>
        ///     Sets the reference that this instance stores to null.
        /// </summary>
        public void MarkForDeletion()
        {
            Reference = null;
            ActionReference = null;
            Method = null;
            _staticAction = null;
        }
    }

    /// <summary>
    ///     Stores an Action without causing a hard reference
    ///     to be created to the Action's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Action's parameter.</typeparam>
    ////[ClassInfo(typeof(WeakAction))]
    public class WeakAction<T> : WeakAction, IExecuteWithObject
    {
        private Action<T> _staticAction;

        /// <summary>
        ///     Initializes a new instance of the WeakAction class.
        /// </summary>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(Action<T> action)
            : this(action?.Target, action)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the WeakAction class.
        /// </summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="action">The action that will be associated to this instance.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "Method should fail with an exception if action is null.")]
        public WeakAction(object target, Action<T> action)
        {
            if (action.GetMethodInfo().IsStatic)
            {
                _staticAction = action;

                if (target != null)
                {
                    // Keep a reference to the target to control the
                    // WeakAction's lifetime.
                    Reference = new WeakReference(target);
                }

                return;
            }

            Method = action.GetMethodInfo();
            ActionReference = new WeakReference(action.Target);
            Reference = new WeakReference(target);
        }

        /// <summary>
        ///     Gets the name of the method that this WeakAction represents.
        /// </summary>
        public override string MethodName => _staticAction != null ? _staticAction.GetMethodInfo().Name : Method.Name;

        /// <summary>
        ///     Gets a value indicating whether the Action's owner is still alive, or if it was collected
        ///     by the Garbage Collector already.
        /// </summary>
        public override bool IsAlive
        {
            get
            {
                if (_staticAction == null
                    && Reference == null)
                {
                    return false;
                }

                if (_staticAction != null)
                {
                    return Reference == null || Reference.IsAlive;
                }

                return Reference.IsAlive;
            }
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
        ///     Sets all the actions that this WeakAction contains to null,
        ///     which is a signal for containing objects that this WeakAction
        ///     should be deleted.
        /// </summary>
        public new void MarkForDeletion()
        {
            _staticAction = null;
            base.MarkForDeletion();
        }

        /// <summary>
        ///     Executes the action. This only happens if the action's owner
        ///     is still alive. The action's parameter is set to default(T).
        /// </summary>
        public new void Execute()
        {
            Execute(default(T));
        }

        /// <summary>
        ///     Executes the action. This only happens if the action's owner
        ///     is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        public void Execute(T parameter)
        {
            if (_staticAction != null)
            {
                _staticAction(parameter);
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null
                    && ActionReference != null
                    && actionTarget != null)
                {
                    Method.Invoke(
                        actionTarget,
                        new object[]
                        {
                            parameter
                        });
                }
            }
        }
    }

    /// <summary>
    ///     This interface is meant for the <see cref="WeakAction{T}" /> class and can be
    ///     useful if you store multiple WeakAction{T} instances but don't know in advance
    ///     what type T represents.
    /// </summary>
    public interface IExecuteWithObject
    {
        /// <summary>
        ///     The target of the WeakAction.
        /// </summary>
        object Target { get; }

        /// <summary>
        ///     Executes an action.
        /// </summary>
        /// <param name="parameter">
        ///     A parameter passed as an object,
        ///     to be casted to the appropriate type.
        /// </param>
        void ExecuteWithObject(object parameter);

        /// <summary>
        ///     Deletes all references, which notifies the cleanup method
        ///     that this entry must be deleted.
        /// </summary>
        void MarkForDeletion();
    }
}