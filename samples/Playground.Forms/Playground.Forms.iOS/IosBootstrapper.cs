// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Playground.Forms.iOS.CustomComponents;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.iOS;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.Services;

namespace Playground.Forms.iOS
{
    public class IosBootstrapper : CoreBootstrapper
    {
        protected override IList<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies()
                .AddItem(GetType().Assembly);
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            base.ConfigureIoc(builder);

            // permissions
            builder.Singleton<PermissionsService, IPermissionsService>();
            builder.Singleton<PermissionsManager, IPermissionsManager>();

            // launcher
            builder.Singleton<ViewControllerProvider, IViewControllerProvider>();
            builder.Singleton<ViewModelToViewMap>();
            builder.Singleton<StoryboardViewLocator, IViewLocator>();
            builder.Singleton<IosLauncherService, ILauncherService>();

            // remote
            // - example of using custom primary http message handler
            builder.Singleton(_ => IosIgnoreSslClientHelper.CreateHandler());
        }
    }
}
