// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms
{
    public abstract class FormsBootstrapper : BootstrapperBase
    {
        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            base.RegisterInternalServices(builder);

            builder.PerDependency<FormsFrameNavigationService, IFrameNavigationService>(IfRegistered.Replace);
            builder.PerDependency<FormsNavigationService, IPlatformNavigationService>(IfRegistered.Replace);

            builder.Singleton<FormsViewLocator, IFormsViewLocator>(IfRegistered.Replace);
            builder.Singleton<FormsPageNavigationService, IPageNavigationService>(IfRegistered.Replace);
            builder.Singleton<FormsDialogsService, IDialogsService>(IfRegistered.Replace);
        }

        protected override IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            //init assembly sources for Activator.cs
            AssemblySourceCache.Install();
            AssemblySourceCache.ExtractTypes = assembly =>
                assembly.GetExportedTypes()
                    .Where(t => typeof(Page).IsAssignableFrom(t));
            AssemblySource.Instance.Clear();
            AssemblySource.Instance.AddRange(assemblies);

            //init ui thread helper
            PlatformProvider.Current = new FormsPlatformProvider();

            return base.BuildContainer(builder, assemblies);
        }
    }
}
