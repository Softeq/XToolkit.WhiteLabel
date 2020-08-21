// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;

// Ported from System.Reactive:
// https://github.com/dotnet/reactive/blob/master/Rx.NET/Source/src/System.Reactive/Disposables/Disposable.cs#L19
namespace Softeq.XToolkit.Common.Disposables
{
    /// <summary>
    ///     Represents an Action-based disposable.
    /// </summary>
    public sealed class AnonymousDisposable : IDisposable
    {
        private volatile Action? _dispose;

        /// <summary>
        ///     Constructs a new disposable with the given action used for disposal.
        /// </summary>
        /// <param name="dispose">Disposal action which will be run upon calling Dispose.</param>
        public AnonymousDisposable(Action dispose)
        {
            _dispose = dispose;
        }

        /// <summary>
        ///     Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _dispose == null;

        /// <summary>
        ///     Calls the disposal action if and only if the current instance hasn't been disposed yet.
        /// </summary>
        public void Dispose()
        {
            Interlocked.Exchange(ref _dispose, null)?.Invoke();
        }
    }
}
