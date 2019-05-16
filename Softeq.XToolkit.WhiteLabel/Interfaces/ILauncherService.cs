// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface ILauncherService
    {
        void OpenUrl(string url);
        void OpenDeviceSecuritySettings();
        void OpenAppSettings();
        void OpenVideo(string videoUrl);
        void OpenEmail(string email);
        void OpenPhoneNumber(string number);
    }
}