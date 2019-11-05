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
        private IDryContainer _container = default!;

        internal void Initialize(IDryContainer container)
        {
            _container = container;
        }

        public TService Resolve<TService>()
        {
            return _container.Resolve<TService>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public Lazy<TService> ResolveLazy<TService>()
        {
            return new Lazy<TService>(() => _container.Resolve<TService>());
        }
    }
}
