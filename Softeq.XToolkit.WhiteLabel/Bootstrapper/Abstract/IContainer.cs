// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract
{
    public interface IContainer
    {
        TService Resolve<TService>();

        [Obsolete("Use Resolve<Lazy<TService>> instead.")]
        Lazy<TService> ResolveLazy<TService>();

        object Resolve(Type type);
    }
}
