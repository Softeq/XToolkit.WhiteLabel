// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using DryIoc;
using IDryContainer = DryIoc.IContainer;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers
{
    internal class DryIocContainerAdapter : IContainer
    {
        private IDryContainer _container;

        internal void Initialize(IDryContainer container)
        {
            _container = container;
        }

        public TService Resolve<TService>(params object[] parameters)
        {
            return _container.Resolve<TService>(parameters);
        }

        public Lazy<TService> ResolveLazy<TService>()
        {
            return new Lazy<TService>(() => _container.Resolve<TService>());
        }
    }
}
