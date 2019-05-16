// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Permissions
{
    public interface IPermissionRequestHandler
    {
        void Handle(int requestCode, string[] permissions, object grantResults);
    }
}
