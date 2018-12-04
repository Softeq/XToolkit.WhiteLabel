// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface ITimeTracker
    {
        TimeSpan CurrentTime { get; }
        TimeSpan SummaryTime { get; }

        void Start();
        void Stop();
        void Reset();
    }
}
