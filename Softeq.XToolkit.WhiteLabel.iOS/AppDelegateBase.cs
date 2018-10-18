// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Foundation;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public abstract class AppDelegateBase : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            //init factory for bindings
            BindingExtensions.Initialize(new AppleBindingFactory());

            //init assembly sources for Activator.cs
            AssemblySourceCache.Install();
            AssemblySourceCache.ExtractTypes = assembly =>
                assembly.GetExportedTypes()
                    .Where(t => typeof(UIViewController).IsAssignableFrom(t));
            AssemblySource.Instance.AddRange(SelectAssemblies());

            //init dependencies
            StartScopeForIoc();

            //init ui thread helper
            PlatformProvider.Current = new IosPlatformProvider();

            return true;
        }

        public abstract void ConfigureIoc(ContainerBuilder builder);

        public abstract IList<Assembly> SelectAssemblies();

        private void StartScopeForIoc()
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureIoc(containerBuilder);
            ServiceLocator.StartScope(containerBuilder);
        }
    }
}