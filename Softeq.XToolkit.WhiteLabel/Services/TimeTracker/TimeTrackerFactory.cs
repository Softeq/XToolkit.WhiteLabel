// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Services.TimeTracker
{
    public class TimeTrackerFactory : ITimeTrackerFactory
    {
        public ITimeTracker Create()
        {
            return new SimpleTimeTracker();
        }
    }
}
