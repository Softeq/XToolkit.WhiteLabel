// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.PushNotifications.Abstract;

namespace Softeq.XToolkit.PushNotifications
{
    public class PushTokenStorageService : IPushTokenStorageService
    {
        private readonly string _pushTokenKey = $"{nameof(IPushTokenStorageService)}_push_token_key";

        private readonly IInternalSettings _internalSettings;

        public PushTokenStorageService(IInternalSettings internalSettings)
        {
            _internalSettings = internalSettings;
        }

        /// <inheritdoc />
        public string PushToken
        {
            get => _internalSettings.GetValueOrDefault(_pushTokenKey, default(string));
            set => _internalSettings.AddOrUpdateValue(_pushTokenKey, value);
        }
    }
}
