// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foundation;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public abstract class AppDelegateBase : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        protected abstract IBootstrapper Bootstrapper { get; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            //init factory for bindings
            BindingExtensions.Initialize(new AppleBindingFactory());

            //init assembly sources for Activator.cs
            AssemblySourceCache.Install();
            AssemblySourceCache.ExtractTypes = assembly =>
                assembly.GetExportedTypes()
                    .Where(t => typeof(UIViewController).IsAssignableFrom(t));

            var assemblies = SelectAssemblies();

            AssemblySource.Instance.AddRange(assemblies);

            //init dependencies
            Bootstrapper.Init(assemblies);

            //init ui thread helper
            PlatformProvider.Current = new IosPlatformProvider();

            return true;
        }

        protected abstract IList<Assembly> SelectAssemblies();
    }
}
