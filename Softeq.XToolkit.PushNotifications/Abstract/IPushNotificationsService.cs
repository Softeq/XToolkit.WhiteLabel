// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications.Abstract
{
    public interface IPushNotificationsService
    {
        Task RegisterForPushNotifications();
        Task UnRegisterForPushNotifications();
    }
}
