// Developed by Softeq Development Corporation
// http://www.softeq.com

 using System.Collections.Generic;
using System.Reflection;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public interface IBootstrapper
    {
        void Init(IList<Assembly> assemblies);
    }
}
