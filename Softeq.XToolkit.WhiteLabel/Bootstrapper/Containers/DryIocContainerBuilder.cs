// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using DryIoc;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;
using IDryContainer = DryIoc.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers
{
    /// <summary>
    ///     Builder based on <see href="https://github.com/dadhi/DryIoc">DryIoc</see> container.
    /// </summary>
    public class DryIocContainerBuilder : IContainerBuilder
    {
        private readonly List<Action<IContainer>> _buildActions;

        private IDryContainer _dryContainer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DryIocContainerBuilder"/> class.
        /// </summary>
        public DryIocContainerBuilder()
        {
            _dryContainer = new Container(CreateContainerRules());
            _buildActions = new List<Action<IContainer>>();
        }

        /// <summary>
        ///     Gets <see cref="DryIoc.Rules" /> to alter behavior of internal <see cref="DryIoc.IContainer" />.
        /// </summary>
        /// <returns><see cref="DryIoc.Rules"/> associated with WL Container instance.</returns>
        protected virtual Rules CreateContainerRules()
        {
            return Rules.Default.WithUseInterpretation();
        }

        /// <inheritdoc />
        public void PerDependency<TImplementation, TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
            where TImplementation : TService
        {
            RegisterInternal<TImplementation, TService>(Reuse.Transient, ifRegistered);
        }

        /// <inheritdoc />
        public void PerDependency<TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal<TService>(Reuse.Transient, ifRegistered);
        }

        /// <inheritdoc />
        public void PerDependency<TService>(Func<IContainer, TService> func, IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal(func, Reuse.Transient, ifRegistered);
        }

        /// <inheritdoc />
        public void PerDependency(Type type, IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal(type, Reuse.Transient, ifRegistered);
        }

#pragma warning disable RECS0096 // Type parameter is never used
        /// <inheritdoc />
        public void PerDependency<TService>(
            Func<IContainer, object> func,
#pragma warning restore RECS0096 // Type parameter is never used
            IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal(func, Reuse.Transient, ifRegistered);
        }

        /// <inheritdoc />
        public void Singleton<TImplementation, TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
            where TImplementation : TService
        {
            RegisterInternal<TImplementation, TService>(Reuse.Singleton, ifRegistered);
        }

        /// <inheritdoc />
        public void Singleton<TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal<TService>(Reuse.Singleton, ifRegistered);
        }

        /// <inheritdoc />
        public void Singleton<TService>(Func<IContainer, TService> func, IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            RegisterInternal(func, Reuse.Singleton);
        }

        /// <inheritdoc />
        public void RegisterBuildCallback(Action<IContainer> action)
        {
            _buildActions.Add(action);
        }

        /// <inheritdoc />
        public void Decorator<TImplementation, TService>()
            where TImplementation : TService
        {
            _dryContainer.Register<TService, TImplementation>(setup: Setup.Decorator);
        }

        /// <inheritdoc />
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

        private void RegisterInternal<TService>(
            Func<IContainer, TService> func,
            IReuse reuse,
            IfRegistered ifRegistered = IfRegistered.AppendNew)
        {
            _dryContainer.RegisterDelegate(
                c => func.Invoke(c.Resolve<IContainer>()),
                reuse,
                null,
                MapIfAlreadyRegistered(ifRegistered));
        }

        private IfAlreadyRegistered MapIfAlreadyRegistered(IfRegistered ifRegistered)
        {
            return ifRegistered switch
            {
                IfRegistered.Keep => IfAlreadyRegistered.Keep,
                IfRegistered.Replace => IfAlreadyRegistered.Replace,
                IfRegistered.AppendNew => IfAlreadyRegistered.AppendNewImplementation,
                _ => throw new ArgumentOutOfRangeException(nameof(ifRegistered), ifRegistered, null)
            };
        }
    }
}
