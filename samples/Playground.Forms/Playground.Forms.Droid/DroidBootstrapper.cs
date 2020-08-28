// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Softeq.XToolkit.Common.Droid.Permissions;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

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
        }
    }
}
