// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Playground.Extended;
using Playground.iOS.Extended;
using Playground.iOS.Services;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.Connectivity.iOS;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.iOS;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Essentials.iOS.FullScreenImage;
using Softeq.XToolkit.WhiteLabel.Essentials.iOS.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.iOS
{
    internal class CustomIosBootstrapper : IosBootstrapperBase
    {
        protected override IList<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies() // Softeq.XToolkit.WhiteLabel.iOS
                .AddItem(typeof(FullScreenImageViewController).Assembly) // Softeq.XToolkit.WhiteLabel.Essentials.iOS
                .AddItem(GetType().Assembly); // Playground.iOS
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.Singleton<IosAppInfoService, IAppInfoService>();

            builder.Singleton<StoryboardDialogsService, IDialogsService>();
            builder.Singleton<IosExtendedDialogsService, IExtendedDialogsService>();

            // permissions
            builder.Singleton<PermissionsService, IPermissionsService>();
            builder.Singleton<PermissionsManager, IPermissionsManager>();

            // image picker
            builder.Singleton<NukeImageService, IIosImageService>();
            builder.Singleton<IosImagePickerService, IImagePickerService>();

            // connectivity
            builder.Singleton<IosConnectivityService, IConnectivityService>();
        }
    }
}
