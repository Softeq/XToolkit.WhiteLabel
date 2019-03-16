// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using DryIoc;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class IoCBuilderExtensions
    {
        public static void PerDependency<T1, T2>(this Container container) where T1 : T2
        {
            container.Register<T2, T1>(Reuse.Transient);
        }

        public static void PerDependency<T1>(this Container container)
        {
            container.Register<T1>(Reuse.Transient);
        }

        public static void PerDependency(this Container container, Type type)
        {
            container.Register(type, Reuse.Transient);
        }

        public static void TryPerDependency<T1, T2>(this Container container) where T1 : T2
        {
            container.Register<T2, T1>(Reuse.Transient, ifAlreadyRegistered: IfAlreadyRegistered.Keep);
        }

        public static void TryPerDependency(this Container container, Type type)
        {
            container.Register(type, Reuse.Transient, ifAlreadyRegistered: IfAlreadyRegistered.Keep);
        }

        public static void PerLifeTimeScope<T1>(this Container container, Func<T1> func)
        {
            container.Register(Made.Of(() => func.Invoke()), Reuse.Singleton);
        }

        public static void PerLifetimeScope<T1, T2>(this Container container) where T1 : T2
        {
            container.Register<T2, T1>(Reuse.Singleton);
        }

        public static void PerLifetimeScope<T1>(this Container container)
        {
            container.Register<T1>(Reuse.Singleton);
        }

        public static void TryPerLifeTimeScope<T1, T2>(this Container container) where T1 : T2
        {
            container.Register<T2, T1>(Reuse.Singleton, ifAlreadyRegistered: IfAlreadyRegistered.Keep);
        }
    }
}