// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Runtime;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using System.Linq;

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

        protected override IList<Assembly> SelectAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly =>
                    new[]
                    {
                        "Playground.Droid",
                        "Softeq.XToolkit.Chat.Droid",
                        "Softeq.XToolkit.WhiteLabel.Droid"
                    }
                    .Any(x => x.Equals(assembly.GetName().Name)))
                .ToList();
        }
    }
}
