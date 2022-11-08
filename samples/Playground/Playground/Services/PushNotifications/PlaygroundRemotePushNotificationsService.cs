// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.PushNotifications;
using Softeq.XToolkit.PushNotifications.Abstract;

namespace Playground.Services.PushNotifications
{
    class PlaygroundRemotePushNotificationsService : IRemotePushNotificationsService
    {
        public Task<bool> SendPushNotificationsToken(string pushNotificationsToken)
        {
            return Task.FromResult(true);
        }

        public Task<bool> RemovePushNotificationsToken(string pushNotificationsToken)
        {
            return Task.FromResult(true);
        }
    }
}
