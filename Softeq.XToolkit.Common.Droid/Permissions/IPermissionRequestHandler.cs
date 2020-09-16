// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Droid.Permissions
{
    /// <summary>
    ///     Android request permissions handler (delegated request from ActivityBase).
    /// </summary>
    public interface IPermissionRequestHandler
    {
        /// <summary>
        ///     Callback for the result from requesting permissions.
        /// </summary>
        /// <param name="requestCode">The request code.</param>
        /// <param name="permissions">The requested permissions.</param>
        /// <param name="grantResults">The grant results for the corresponding permissions.</param>
        void Handle(int requestCode, string[] permissions, object grantResults);
    }
}
