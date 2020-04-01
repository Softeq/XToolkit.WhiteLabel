// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

// Ported from System.Reactive:
// https://github.com/dotnet/reactive/blob/master/Rx.NET/Source/src/System.Reactive/Disposables/AnonymousDisposable.cs#L12
namespace Softeq.XToolkit.Common.Disposables
{
    /// <summary>
    ///    Provides a set of static methods for creating <see cref="IDisposable"/> objects.
    /// </summary>
    public static class Disposable
    {
        /// <summary>
        ///     Creates a disposable object that invokes the specified action when disposed.
        /// </summary>
        /// <param name="dispose">
        ///     Action to run during the first call to <see cref="IDisposable.Dispose"/>.
        ///     The action is guaranteed to be run at most once.
        /// </param>
        /// <returns>The disposable object that runs the given action upon disposal.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is <c>null</c>.</exception>
        public static IDisposable Create(Action dispose)
        {
            if (dispose == null)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            return new AnonymousDisposable(dispose);
        }
    }
}
