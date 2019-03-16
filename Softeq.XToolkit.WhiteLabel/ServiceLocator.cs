// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using DryIoc;

namespace Softeq.XToolkit.WhiteLabel
{
    public interface IIocContainer
    {
        void StartScope(Container builder);
        void RestartScope();
        T Resolve<T>();
        object Resolve(Type type);
        bool IsRegistered<T>();
        bool IsRegistered(Type type);
        Lazy<T> LazyResolve<T>();
    }

    public class IocContainer : IIocContainer
    {
        private Container _currentScope;
        private Container _containerBuilder;

        public void StartScope(Container builder)
        {
            _containerBuilder = builder;
            Initialize(_containerBuilder);
        }

        public void RestartScope()
        {
            _currentScope.Dispose();

            Initialize(_containerBuilder);
        }

        public T Resolve<T>()
        {
            return _currentScope.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _currentScope.Resolve(type);
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

        private void Initialize(Container builder)
        {
            _currentScope = builder;
        }
    }
}