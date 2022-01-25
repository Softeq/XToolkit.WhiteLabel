// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Playground.Forms.Droid.CustomComponents;
using Softeq.XToolkit.Common.Droid.Permissions;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Playground.Forms.Droid
{
    public class DroidBootstrapper : CoreBootstrapper
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
            builder.Singleton<RequestResultHandler, IPermissionRequestHandler>();

            // launcher
            // YP: conflict between Google.Android.Material from Xamarin.Forms (latest) and WL.Droid (old stable)
            // builder.Singleton<EssentialsContextProvider, IContextProvider>();
            // builder.Singleton<DroidLauncherService, ILauncherService>();
            builder.Singleton<EssentialsLauncherService, ILauncherService>();

            // remote
            // - example of using custom primary http message handler
            builder.Singleton<HttpMessageHandler>(_ => new DroidIgnoreSslClientHandler());
        }
    }
}
