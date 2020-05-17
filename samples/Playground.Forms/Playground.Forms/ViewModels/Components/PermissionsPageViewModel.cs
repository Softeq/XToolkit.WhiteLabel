// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Playground.ViewModels.Components;
using Plugin.Permissions;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.ViewModels.Components
{
    public class PermissionsPageViewModel : ViewModelBase
    {
        public PermissionsPageViewModel(IPermissionsManager permissionsManager)
        {
            CheckAllCommand = new AsyncCommand<EventArgs>(CheckAll);

            Photos = new PermissionViewModel<PhotosPermission>(permissionsManager);
        }

        public ICommand CheckAllCommand { get; }

        public PermissionViewModel<PhotosPermission> Photos { get; }

        private async Task CheckAll(EventArgs _)
        {
            await Photos.CheckStatus();
        }
    }
}
