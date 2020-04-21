// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Droid.Permissions
{
    /// <summary>
    ///     Android request permissions handler (delegated request from ActivityBase).
    /// </summary>
    public interface IPermissionRequestHandler
    {
        void Handle(int requestCode, string[] permissions, object grantResults);
    }
}
