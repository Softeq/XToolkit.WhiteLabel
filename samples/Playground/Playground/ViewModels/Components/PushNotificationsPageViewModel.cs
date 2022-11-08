// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.PushNotifications;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Components
{
    public class PushNotificationsPageViewModel : ViewModelBase
    {
        private readonly IPushNotificationsService _pushNotificationsService;

        public PushNotificationsPageViewModel(
            IPushNotificationsService pushNotificationsService)
        {
            _pushNotificationsService = pushNotificationsService;

            RegisterCommand = new AsyncCommand(Register);
            UnregisterCommand = new AsyncCommand(Unregister);
        }

        public ICommand RegisterCommand { get; }

        public IAsyncCommand UnregisterCommand { get; }

        private async Task Register()
        {
            // var result =
            await _pushNotificationsService.RegisterForPushNotificationsAsync();
        }

        private async Task Unregister()
        {
            await _pushNotificationsService.UnregisterForPushNotificationsAsync();
            //PushNotificationsUnregisterOptions.InSystemAndOnServer
        }
    }
}
