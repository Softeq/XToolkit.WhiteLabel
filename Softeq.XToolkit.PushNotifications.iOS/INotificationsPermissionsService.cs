// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.PushNotifications.iOS
{
    public interface INotificationsPermissionsService
    {
        //bool SetRequiredOptions(UNAuthorizationOptions[] options);
        Task<bool> RequestNotificationsPermissions();
    }
}
