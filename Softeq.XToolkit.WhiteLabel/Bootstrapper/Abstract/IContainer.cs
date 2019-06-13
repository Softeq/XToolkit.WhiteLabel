// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract
{
    public interface IContainer
    {
        T Resolve<T>();
        object Resolve(Type type);
    }
}
