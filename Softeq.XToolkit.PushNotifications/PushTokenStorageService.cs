// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.PushNotifications
{
    public class PushTokenStorageService : IPushTokenStorageService
    {
        private readonly IInternalSettings _internalSettings;

        public PushTokenStorageService(IInternalSettings internalSettings)
        {
            _internalSettings = internalSettings;
        }

        private const string PushTokenKey = "push_token_key";
        public string PushToken
        {
            get => _internalSettings.GetValueOrDefault(PushTokenKey, default(string));
            set => _internalSettings.AddOrUpdateValue(PushTokenKey, value);
        }
    }
}
