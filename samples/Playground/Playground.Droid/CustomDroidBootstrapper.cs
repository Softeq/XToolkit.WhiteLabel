// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Playground.Droid.Extended;
using Playground.Droid.Services;
using Playground.Extended;
using Softeq.XToolkit.Common.Droid.Permissions;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Interfaces;
using Softeq.XToolkit.WhiteLabel.Essentials.Droid.FullScreenImage;
using Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Navigation;
using IImagePickerService = Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker.IImagePickerService;

namespace Playground.Droid
{
    internal class CustomDroidBootstrapper : DroidBootstrapperBase
    {
        protected override IList<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies() // Softeq.XToolkit.WhiteLabel.Droid
                .AddItem(typeof(FullScreenImageDialogFragment).Assembly) // Softeq.XToolkit.WhiteLabel.Essentials.Droid
                .AddItem(GetType().Assembly); // Playground.Droid
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.Singleton<DroidFragmentDialogService, IDialogsService>();
            builder.Singleton<DroidExtendedDialogsService, IExtendedDialogsService>();

            // permissions
            builder.Singleton<PermissionsManager, IPermissionsManager>();
            builder.Singleton<RequestResultHandler, IPermissionRequestHandler>();

            // image picker
            builder.Singleton<GlideImageService, IDroidImageService>();
            builder.Singleton<DroidImagePickerService, IImagePickerService>();
            builder.Singleton<ImagePickerActivityResultHandler, IImagePickerActivityResultHandler>();

            // connectivity
            builder.Singleton<IConnectivityService>(_ => EssentialsConnectivityService.Default());
        }
    }
}
