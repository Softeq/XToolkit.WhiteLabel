using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Softeq.XToolkit.Permissions
{
    public class NotificationsPermission : BasePermission
    {
        public NotificationsPermission() : base(Permission.Unknown)
        {
        }
    }
}
