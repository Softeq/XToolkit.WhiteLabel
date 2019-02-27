// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Autofac;
using Autofac.Builder;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class AutofacBuilderExtensions
    {
        public static IRegistrationBuilder<T1, ConcreteReflectionActivatorData, SingleRegistrationStyle> PerDependency<T1, T2>(this ContainerBuilder builder) where T1 : T2
        {
            return builder.RegisterType<T1>().As<T2>().InstancePerDependency();
        }

        public static IRegistrationBuilder<T1, ConcreteReflectionActivatorData, SingleRegistrationStyle> PerDependency<T1>(this ContainerBuilder builder)
        {
            return builder.RegisterType<T1>().InstancePerDependency();
        }

        public static void PerDependency<T1>(this ContainerBuilder builder, Func<IComponentContext, T1> func)
        {
            builder.Register(func).InstancePerDependency();
        }

        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> PerDependency(this ContainerBuilder builder, Type type)
        {
            return builder.RegisterType(type).InstancePerDependency();
        }

        public static void PerDependency<T1>(this ContainerBuilder builder, Func<IComponentContext, object> func)
        {
            builder.Register(func).As<T1>().InstancePerDependency();
        }

        public static IRegistrationBuilder<T1, ConcreteReflectionActivatorData, SingleRegistrationStyle> PerLifetimeScope<T1, T2>(this ContainerBuilder builder) where T1 : T2
        {
            return builder.RegisterType<T1>().As<T2>().InstancePerLifetimeScope();
        }

        public static IRegistrationBuilder<T1, ConcreteReflectionActivatorData, SingleRegistrationStyle> PerLifetimeScope<T1>(this ContainerBuilder builder)
        {
            return builder.RegisterType<T1>().InstancePerLifetimeScope();
        }

        public static IRegistrationBuilder<T1, SimpleActivatorData, SingleRegistrationStyle> PerLifetimeScope<T1>(this ContainerBuilder builder, Func<IComponentContext, T1> func)
        {
            return builder.Register(func).InstancePerLifetimeScope();
        }
    }
}