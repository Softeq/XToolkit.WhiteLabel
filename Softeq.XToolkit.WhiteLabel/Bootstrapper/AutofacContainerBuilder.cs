// Developed for PAWS-HALO by Softeq Development
// Corporation http://www.softeq.com

using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using IContainer = Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public class AutofacContainerBuilder : IContainerBuilder
    {
        private readonly ContainerBuilder _builder;
        private readonly List<Action<IContainer>> _buildActions;

        public AutofacContainerBuilder()
        {
            _builder = new ContainerBuilder();
            _buildActions = new List<Action<IContainer>>();
        }

        public IConcreteRegistration PerDependency<T1, T2>() where T1 : T2
        {
            return new AutofacRegistration<T1>(_builder.RegisterType<T1>().As<T2>().InstancePerDependency());
        }

        public IConcreteRegistration PerDependency<T1>()
        {
            return new AutofacRegistration<T1>(_builder.RegisterType<T1>().InstancePerDependency());
        }

        public IConcreteRegistration PerDependency<T1>(Func<IContainer, T1> func)
        {
            return new AutofacRegistration<T1>(_builder.Register(c => func.Invoke(c.Resolve<IContainer>()))
                .InstancePerDependency());
        }

        public IConcreteRegistration PerDependency(Type type)
        {
            return new AutofacRegistration(_builder.RegisterType(type).InstancePerDependency());
        }

        public IConcreteRegistration PerDependency<T1>(Func<IContainer, object> func)
        {
            return new AutofacRegistration<T1>(_builder.Register(c => func.Invoke(c.Resolve<IContainer>())).As<T1>()
                .InstancePerDependency());
        }

        public IConcreteRegistration Singleton<T1, T2>() where T1 : T2
        {
            return new AutofacRegistration<T1>(_builder.RegisterType<T1>().As<T2>().InstancePerLifetimeScope().SingleInstance());
        }

        public IConcreteRegistration Singleton<T1>()
        {
            return new AutofacRegistration<T1>(_builder.RegisterType<T1>().InstancePerLifetimeScope().SingleInstance());
        }

        public IConcreteRegistration Singleton<T1>(Func<IContainer, T1> func)
        {
            return new AutofacRegistration<T1>(_builder.Register(c => func.Invoke(c.Resolve<IContainer>()))
                .InstancePerLifetimeScope().SingleInstance());
        }

        public void RegisterBuildCallback(Action<IContainer> action)
        {
            _buildActions.Add(action);
        }

        public IContainer Build()
        {
            Autofac.IContainer autofacContainer = null;
            Singleton(c => new AutofacContainer().Initialize(autofacContainer));

            autofacContainer = _builder.Build();
            var container = autofacContainer.Resolve<IContainer>();

            _buildActions.Apply(action => action?.Invoke(container));
            _buildActions.Clear();

            return container;
        }
    }

    internal class AutofacRegistration<T1> : IConcreteRegistration
    {
        private readonly IRegistrationBuilder<T1, IConcreteActivatorData, SingleRegistrationStyle>
            _instance;

        private readonly IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> _objectInstance;

        public AutofacRegistration(IRegistrationBuilder<T1, IConcreteActivatorData, SingleRegistrationStyle> instance)
        {
            _instance = instance;
        }

        public AutofacRegistration(IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> instance)
        {
            _objectInstance = instance;
        }

        public void PreserveExistingDefaults()
        {
            _instance?.PreserveExistingDefaults();
        }
    }

    internal class AutofacRegistration : IConcreteRegistration
    {
        private readonly IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> _objectInstance;

        public AutofacRegistration(
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> instance)
        {
            _objectInstance = instance;
        }

        public void PreserveExistingDefaults()
        {
            _objectInstance?.PreserveExistingDefaults();
        }
    }

    internal class AutofacContainer : IContainer
    {
        private Autofac.IContainer _container;

        public IContainer Initialize(Autofac.IContainer container)
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
    }
}
