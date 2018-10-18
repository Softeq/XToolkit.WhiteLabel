// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Autofac;
using Autofac.Core;

namespace Softeq.XToolkit.WhiteLabel
{
    public static class ServiceLocator
    {
        private static ILifetimeScope _currentScope;
        private static ILifetimeScope _rootScope;
        private static ContainerBuilder _containerBuilder;

        public static void StartScope(ContainerBuilder builder)
        {
            _containerBuilder = builder;
            Initialize(_containerBuilder);
        }

        public static void RestartScope()
        {
            _rootScope.Dispose();
            _currentScope.Dispose();

            Initialize(_containerBuilder);
        }

        public static T Resolve<T>(params Parameter[] par)
        {
            return _currentScope.Resolve<T>(par);
        }

        public static T Resolve<T>()
        {
            return _currentScope.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return _currentScope.Resolve(type);
        }

        public static object Resolve(Type type, params Parameter[] par)
        {
            return _currentScope.Resolve(type, par);
        }

        public static bool IsRegistered<T>()
        {
            return _currentScope.IsRegistered(typeof(T));
        }

        public static bool IsRegistered(Type type)
        {
            return _currentScope.IsRegistered(type);
        }

        public static Lazy<T> LazyResolve<T>()
        {
            return new Lazy<T>(Resolve<T>);
        }

        public static Lazy<T> LazyResolve<T>(params Parameter[] par)
        {
            return new Lazy<T>(() => Resolve<T>(par));
        }

        private static void Initialize(ContainerBuilder builder)
        {
            _rootScope = builder.Build();
            _currentScope = _rootScope.BeginLifetimeScope();
        }
    }
}