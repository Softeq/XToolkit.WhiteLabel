// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Autofac;
using Autofac.Core;
using Softeq.XToolkit.Common.Files;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel
{
    public interface IIocContainer
    {
        void StartScope(ContainerBuilder builder);
        void RestartScope();
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
        private ILifetimeScope _currentScope;
        private ILifetimeScope _rootScope;
        private ContainerBuilder _containerBuilder;

        public void StartScope(ContainerBuilder builder)
        {
            _containerBuilder = builder;
            Initialize(_containerBuilder);
        }

        public void RestartScope()
        {
            _rootScope.Dispose();
            _currentScope.Dispose();

            Initialize(_containerBuilder);
        }

        public T Resolve<T>(params Parameter[] par)
        {
            return _currentScope.Resolve<T>(par);
        }

        public T Resolve<T>()
        {
            return _currentScope.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _currentScope.Resolve(type);
        }

        public object Resolve(Type type, params Parameter[] par)
        {
            return _currentScope.Resolve(type, par);
        }

        public bool IsRegistered<T>()
        {
            return _currentScope.IsRegistered(typeof(T));
        }

        public bool IsRegistered(Type type)
        {
            return _currentScope.IsRegistered(type);
        }

        public Lazy<T> LazyResolve<T>()
        {
            return new Lazy<T>(Resolve<T>);
        }

        public Lazy<T> LazyResolve<T>(params Parameter[] par)
        {
            return new Lazy<T>(() => Resolve<T>(par));
        }

        private void Initialize(ContainerBuilder builder)
        {
            _rootScope = builder.Build();
            _currentScope = _rootScope.BeginLifetimeScope();
        }
    }

    public static class Dependencies
    {
        private static bool _isInitialized;
        private static IIocContainer _iocContainer;

        public static void Initialize(IIocContainer iocContainer)
        {
            if (_isInitialized)
            {
                throw new ArgumentException($"{nameof(Dependencies)} already initialized");
            }

            _iocContainer = iocContainer;
            _isInitialized = true;
        }

        public static IIocContainer IocContainer => _iocContainer;
        
        public static IPageNavigationService PageNavigationService => IocContainer.Resolve<IPageNavigationService>();

        public static IPermissionRequestHandler PermissionRequestHandler =>
            IocContainer.Resolve<IPermissionRequestHandler>();

        public static IJsonSerializer JsonSerializer => IocContainer.Resolve<IJsonSerializer>();
        public static IFilesProvider InternalStorageProvider => IocContainer.Resolve<InternalStorageProvider>();
        public static IPermissionsManager PermissionsManager => IocContainer.Resolve<IPermissionsManager>();
    }
}