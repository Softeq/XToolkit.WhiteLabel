// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract
{
    public interface IContainerBuilder
    {
        void PerDependency<T1, T2>(IfRegistered ifRegistered = IfRegistered.AppendNewImplementation) where T1 : T2;
        void PerDependency<T1>(IfRegistered ifRegistered = IfRegistered.AppendNewImplementation);
        void PerDependency<T1>(Func<IContainer, T1> func, IfRegistered ifRegistered = IfRegistered.AppendNewImplementation);
        void PerDependency(Type type, IfRegistered ifRegistered = IfRegistered.AppendNewImplementation);
        void PerDependency<T1>(Func<IContainer, object> func, IfRegistered ifRegistered = IfRegistered.AppendNewImplementation);
        void Singleton<T1, T2>(IfRegistered ifRegistered = IfRegistered.AppendNewImplementation) where T1 : T2;
        void Singleton<T1>(IfRegistered ifRegistered = IfRegistered.AppendNewImplementation);
        void Singleton<T1>(Func<IContainer, T1> func, IfRegistered ifRegistered = IfRegistered.AppendNewImplementation);
        void RegisterBuildCallback(Action<IContainer> action);
        IContainer Build();
    }
}
