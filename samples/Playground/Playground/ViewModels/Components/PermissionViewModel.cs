// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Playground.ViewModels.Components
{
    public class PermissionViewModel<TPermission> : ObservableObject
        where TPermission : BasePermission, new()
    {
        private readonly IPermissionsManager _permissionsManager;

        private bool _isGranted;

        public PermissionViewModel(IPermissionsManager permissionsManager)
        {
            _permissionsManager = permissionsManager;

            RequestPermissionCommand = new AsyncCommand(RequestPermission);
        }

        public bool IsGranted
        {
            get => _isGranted;
            private set => Set(ref _isGranted, value);
        }

        public ICommand RequestPermissionCommand { get; }

        public async Task CheckStatus()
        {
            var status = await _permissionsManager.CheckAsync<TPermission>().ConfigureAwait(false);

            Execute.OnUIThread(() =>
            {
                IsGranted = status == PermissionStatus.Granted;
            });
        }

        private async Task RequestPermission()
        {
            await _permissionsManager.CheckWithRequestAsync<TPermission>().ConfigureAwait(false);

            await CheckStatus();
        }
    }
}
