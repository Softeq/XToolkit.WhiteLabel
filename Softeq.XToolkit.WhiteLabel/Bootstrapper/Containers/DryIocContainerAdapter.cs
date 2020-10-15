// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using DryIoc;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;
using IDryContainer = DryIoc.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers
{
    internal class DryIocContainerAdapter : IContainer
    {
        private IDryContainer _container = default!;

        internal void Initialize(IDryContainer container)
        {
            _container = container;
        }

        /// <inheritdoc/>
        public TService Resolve<TService>(params object[] parameters) where TService : notnull
        {
            return _container.Resolve<TService>(parameters);
        }

        public Lazy<TService> ResolveLazy<TService>() where TService : notnull
        {
            return new Lazy<TService>(() => _container.Resolve<TService>());
        }
    }
}
