// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IAnalyticsService
    {
        void TrackEvent(string eventName, Dictionary<string, string> properties = null);
        void TrackError(Exception exception, Dictionary<string, string> properties = null);
    }
}
