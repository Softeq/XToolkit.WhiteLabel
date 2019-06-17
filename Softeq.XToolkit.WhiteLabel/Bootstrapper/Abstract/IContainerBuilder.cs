// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract
{
    public interface IContainerBuilder
    {
        IConcreteRegistration PerDependency<T1, T2>() where T1 : T2;
        IConcreteRegistration PerDependency<T1>();
        IConcreteRegistration PerDependency<T1>(Func<IContainer, T1> func);
        IConcreteRegistration PerDependency(Type type);
        IConcreteRegistration PerDependency<T1>(Func<IContainer, object> func);
        IConcreteRegistration Singleton<T1, T2>() where T1 : T2;
        IConcreteRegistration Singleton<T1>();
        IConcreteRegistration Singleton<T1>(Func<IContainer, T1> func);
        void RegisterBuildCallback(Action<IContainer> action);
        IContainer Build();
    }
}
