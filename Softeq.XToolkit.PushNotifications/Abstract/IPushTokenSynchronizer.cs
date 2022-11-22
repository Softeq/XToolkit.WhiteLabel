// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications.Abstract
{
    public interface IPushTokenSynchronizer
    {
        Task OnRegisterSuccessInternalAsync(string token);

        void OnRegisterFailedInternal();

        Task ResendTokenToServerIfNeedAsync();
    }
}
