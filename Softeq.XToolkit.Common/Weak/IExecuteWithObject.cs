// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Weak
{
    /// <summary>
    ///     This interface is meant for the <see cref="WeakAction{T}" /> class and can be
    ///     useful if you store multiple WeakAction{T} instances but don't know in advance
    ///     what type T represents.
    /// </summary>
    public interface IExecuteWithObject
    {
        /// <summary>
        ///     Gets a value indicating whether the Delegate's owner is still alive, or if it was collected
        ///     by the Garbage Collector already.
        /// </summary>
        public bool IsAlive { get; }

        /// <summary>
        ///     Gets the target of the WeakAction.
        /// </summary>
        object? Target { get; }

        /// <summary>
        ///     Gets the name of the method that this WeakDelegate represents.
        /// </summary>
        public string? MethodName { get; }

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
