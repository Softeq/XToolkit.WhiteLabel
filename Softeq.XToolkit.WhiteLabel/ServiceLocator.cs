// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Autofac;
using Autofac.Core;

namespace Softeq.XToolkit.WhiteLabel
{
    public interface IIocContainer
    {
        void StartScope(ContainerBuilder builder);
        T Resolve<T>(params Parameter[] par);
        T Resolve<T>();
        object Resolve(Type type);
        object Resolve(Type type, params Parameter[] par);
        bool IsRegistered<T>();
        bool IsRegistered(Type type);
        Lazy<T> LazyResolve<T>();
        Lazy<T> LazyResolve<T>(params Parameter[] par);
    }

    public class IocContainer : IIocContainer
    {
        private ILifetimeScope _rootScope;
        private ContainerBuilder _containerBuilder;

        public void StartScope(ContainerBuilder builder)
        {
            _containerBuilder = builder;
            _rootScope = builder.Build();
        }

        public T Resolve<T>(params Parameter[] par)
        {
            return _rootScope.Resolve<T>(par);
        }

        public T Resolve<T>()
        {
            return _rootScope.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _rootScope.Resolve(type);
        }

        public object Resolve(Type type, params Parameter[] par)
        {
            return _rootScope.Resolve(type, par);
        }

        public bool IsRegistered<T>()
        {
            return _rootScope.IsRegistered(typeof(T));
        }

        public bool IsRegistered(Type type)
        {
            return _rootScope.IsRegistered(type);
        }

        public Lazy<T> LazyResolve<T>()
        {
            return new Lazy<T>(Resolve<T>);
        }

        public Lazy<T> LazyResolve<T>(params Parameter[] par)
        {
            return new Lazy<T>(() => Resolve<T>(par));
        }
    }
}