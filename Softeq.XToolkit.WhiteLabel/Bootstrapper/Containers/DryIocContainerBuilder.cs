// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using DryIoc;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using IDryContainer = DryIoc.IContainer;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers
{
    public class DryIocContainerBuilder : IContainerBuilder
    {
        private readonly List<Action<IContainer>> _buildActions;
        private IDryContainer _dryContainer;

        public DryIocContainerBuilder()
        {
            _dryContainer = new Container(rules => rules.WithoutFastExpressionCompiler());
            _buildActions = new List<Action<IContainer>>();
        }

        public void PerDependency<TImplementation, TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
            where TImplementation : TService
        {
            RegisterInternal<TImplementation, TService>(Reuse.Transient, ifRegistered);
        }

        public void PerDependency<TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal<TService>(Reuse.Transient, ifRegistered);
        }

        public void PerDependency<TService>(Func<IContainer, TService> func, IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal(func, Reuse.Transient, ifRegistered);
        }

        public void PerDependency(Type type, IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal(type, Reuse.Transient, ifRegistered);
        }

#pragma warning disable RECS0096 // Type parameter is never used
        public void PerDependency<TService>(Func<IContainer, object> func,
#pragma warning restore RECS0096 // Type parameter is never used
            IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal(func, Reuse.Transient, ifRegistered);
        }

        public void Singleton<TImplementation, TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
            where TImplementation : TService
        {
            RegisterInternal<TImplementation, TService>(Reuse.Singleton, ifRegistered);
        }

        public void Singleton<TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal<TService>(Reuse.Singleton, ifRegistered);
        }

        public void Singleton<TService>(Func<IContainer, TService> func, IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal(func, Reuse.Singleton);
        }

        public void RegisterBuildCallback(Action<IContainer> action)
        {
            _buildActions.Add(action);
        }

        public void RegisterAsDecorator<TService, TImplementation>()
            where TImplementation : TService
        {
            _dryContainer.Register<TService, TImplementation>(setup: Setup.Decorator);
        }

        public IContainer Build()
        {
            var container = new DryIocContainerAdapter();

            _dryContainer.RegisterInstance<IContainer>(container, IfAlreadyRegistered.Keep);

            _dryContainer = _dryContainer.WithNoMoreRegistrationAllowed(); // lock for new registrations

            container.Initialize(_dryContainer);

            _buildActions.Apply(action => action?.Invoke(container));
            _buildActions.Clear();

            return container;
        }

        private void RegisterInternal<TService>(IReuse reuse, IfRegistered ifRegistered)
        {
            _dryContainer.Register<TService>(reuse, null, null, MapIfAlreadyRegistered(ifRegistered));
        }

        private void RegisterInternal<TImplementation, TService>(IReuse reuse, IfRegistered ifRegistered)
            where TImplementation : TService
        {
            _dryContainer.Register<TService, TImplementation>(reuse, null, null, MapIfAlreadyRegistered(ifRegistered));
        }

        private void RegisterInternal(Type type, IReuse reuse, IfRegistered ifRegistered)
        {
            _dryContainer.Register(type, reuse, null, null, MapIfAlreadyRegistered(ifRegistered));
        }

        private void RegisterInternal<TService>(Func<IContainer, TService> func, IReuse reuse,
            IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            _dryContainer.RegisterDelegate(c => func.Invoke(c.Resolve<IContainer>()), reuse, null,
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
                case IfRegistered.AppendNew:
                    return IfAlreadyRegistered.AppendNewImplementation;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ifRegistered), ifRegistered, null);
            }
        }
    }
}
