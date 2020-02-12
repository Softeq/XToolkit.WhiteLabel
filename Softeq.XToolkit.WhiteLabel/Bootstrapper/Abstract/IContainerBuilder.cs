// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract
{
    public interface IContainerBuilder
    {
        void PerDependency<TImplementation, TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
            where TImplementation : TService;

        void PerDependency<TService>(IfRegistered ifRegistered = IfRegistered.AppendNew);

        void PerDependency<TService>(Func<IContainer, TService> func, IfRegistered ifRegistered = IfRegistered.AppendNew);

        void PerDependency(Type type, IfRegistered ifRegistered = IfRegistered.AppendNew);

        void PerDependency<TService>(Func<IContainer, object> func, IfRegistered ifRegistered = IfRegistered.AppendNew);

        void Singleton<TImplementation, TService>(IfRegistered ifRegistered = IfRegistered.AppendNew)
            where TImplementation : TService;

        void Singleton<TService>(IfRegistered ifRegistered = IfRegistered.AppendNew);

        void Singleton<TService>(Func<IContainer, TService> func, IfRegistered ifRegistered = IfRegistered.AppendNew);

        void RegisterBuildCallback(Action<IContainer> action);

        void RegisterAsDecorator<TService, TImplementation>()
            where TImplementation : TService;

        IContainer Build();
    }
}
