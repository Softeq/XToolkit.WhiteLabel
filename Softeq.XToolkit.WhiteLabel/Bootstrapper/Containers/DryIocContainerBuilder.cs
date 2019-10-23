// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using DryIoc;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers
{
    public class DryIocContainerBuilder : IContainerBuilder
    {
        private readonly List<Action<IContainer>> _buildActions;
        private DryIoc.IContainer _container;

        public DryIocContainerBuilder()
        {
            _container = new Container(rules => rules.WithoutFastExpressionCompiler());
            _buildActions = new List<Action<IContainer>>();
        }

        public void PerDependency<T1, T2>(IfRegistered ifRegistered = IfRegistered.AppendNewImplementation) where T1 : T2
        {
            RegisterInternal<T1, T2>(Reuse.Transient, ifRegistered);
        }

        public void PerDependency<T1>(IfRegistered ifRegistered = IfRegistered.AppendNewImplementation)
        {
            RegisterInternal<T1>(Reuse.Transient, ifRegistered);
        }

        public void PerDependency<T1>(Func<IContainer, T1> func, IfRegistered ifRegistered = IfRegistered.AppendNewImplementation)
        {
            RegisterInternal(func, Reuse.Transient, ifRegistered);
        }

        public void PerDependency(Type type, IfRegistered ifRegistered = IfRegistered.AppendNewImplementation)
        {
            RegisterInternal(type, Reuse.Transient, ifRegistered);
        }

        public void PerDependency<T1>(Func<IContainer, object> func,
            IfRegistered ifRegistered = IfRegistered.AppendNewImplementation)
        {
            RegisterInternal(func, Reuse.Transient, ifRegistered);
        }

        public void Singleton<T1, T2>(IfRegistered ifRegistered = IfRegistered.AppendNewImplementation) where T1 : T2
        {
            RegisterInternal<T1, T2>(Reuse.Singleton, ifRegistered);
        }

        public void Singleton<T1>(IfRegistered ifRegistered = IfRegistered.AppendNewImplementation)
        {
            RegisterInternal<T1>(Reuse.Singleton, ifRegistered);
        }

        public void Singleton<T1>(Func<IContainer, T1> func, IfRegistered ifRegistered = IfRegistered.AppendNewImplementation)
        {
            RegisterInternal(func, Reuse.Singleton);
        }

        public void RegisterBuildCallback(Action<IContainer> action)
        {
            _buildActions.Add(action);
        }

        public IContainer Build()
        {
            Singleton<DryIocContainer, IContainer>();

            var container = _container.Resolve<IContainer>();
            _container = _container.WithNoMoreRegistrationAllowed();

            ((DryIocContainer) container).Initialize(_container);

            _buildActions.Apply(action => action?.Invoke(container));
            _buildActions.Clear();

            return container;
        }

        private void RegisterInternal<T1>(IReuse reuse, IfRegistered ifRegistered)
        {
            _container.Register<T1>(reuse, null, null, MapIfAlreadyRegistered(ifRegistered));
        }

        private void RegisterInternal<T1, T2>(IReuse reuse, IfRegistered ifRegistered)
            where T1 : T2
        {
            _container.Register<T2, T1>(reuse, null, null, MapIfAlreadyRegistered(ifRegistered));
        }

        private void RegisterInternal(Type type, IReuse reuse, IfRegistered ifRegistered)
        {
            _container.Register(type, reuse, null, null, MapIfAlreadyRegistered(ifRegistered));
        }

        private void RegisterInternal<T1>(Func<IContainer, T1> func, IReuse reuse,
            IfRegistered ifRegistered = IfRegistered.AppendNewImplementation)
        {
            _container.RegisterDelegate(c => func.Invoke(c.Resolve<IContainer>()), reuse, null,
                MapIfAlreadyRegistered(ifRegistered));
        }

        private IfAlreadyRegistered MapIfAlreadyRegistered(IfRegistered ifRegistered)
        {
            switch (ifRegistered)
            {
                case IfRegistered.Keep:
                    return IfAlreadyRegistered.Keep;
                case IfRegistered.Replace:
                    return IfAlreadyRegistered.Replace;
                case IfRegistered.AppendNewImplementation:
                    return IfAlreadyRegistered.AppendNewImplementation;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ifRegistered), ifRegistered, null);
            }
        }
    }
}
