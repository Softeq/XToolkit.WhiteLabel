// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract
{
    /// <summary>
    ///     IoC container interface that is used to resolve dependencies.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        ///     Gets the instance of the specified service with the specified parameters.
        /// </summary>
        /// <typeparam name="TService">
        ///     Type of service to resolve.
        /// </typeparam>
        /// <param name="parameters">
        ///     Service parameters.
        /// </param>
        /// <returns>
        ///     Instance of the <typeparamref name="TService"/>.
        /// </returns>
        TService Resolve<TService>(params object[] parameters) where TService : notnull;

        [Obsolete("Use Resolve<Lazy<TService>> instead.")]
        Lazy<TService> ResolveLazy<TService>() where TService : notnull;
    }
}
