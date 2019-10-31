// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using DryIoc;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers
{
    internal class DryIocContainer : IContainer
    {
        private DryIoc.IContainer _container = default!;

        public IContainer Initialize(DryIoc.IContainer container)
        {
            _container = container;
            return this;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public Lazy<T> ResolveLazy<T>()
        {
            return new Lazy<T>(() => _container.Resolve<T>());
        }
    }
}
