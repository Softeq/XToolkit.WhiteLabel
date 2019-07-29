// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Runtime;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid
{
#if DEBUG
    [Application(Debuggable = true)]
#else
    [Application(Debuggable = false)]
#endif
    public class MainApplication : MainApplicationBase
    {
        protected MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        protected override IBootstrapper Bootstrapper => new CustomDroidBootstrapper();

        protected override IList<Assembly> SelectAssemblies() => new List<Assembly>
        {
            GetType().Assembly,                  // Playground.Droid
            typeof(MainApplicationBase).Assembly // Softeq.XToolkit.WhiteLabel.Droid
        };
    }
}
