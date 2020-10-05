// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract
{
    /// <summary>
    ///     DI container interface that is used for resolving dependencies.
    /// </summary>
    public interface IContainer
    {
        TService Resolve<TService>(params object[] parameters) where TService : notnull;

        [Obsolete("Use Resolve<Lazy<TService>> instead.")]
        Lazy<TService> ResolveLazy<TService>() where TService : notnull;
    }
}
