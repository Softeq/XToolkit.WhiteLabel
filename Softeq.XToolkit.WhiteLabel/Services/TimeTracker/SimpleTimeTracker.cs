// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Services.TimeTracker
{
    public class SimpleTimeTracker : ITimeTracker
    {
        private const string WasNotStopped = "Tracking wasn't stopped!";

        private bool _isWorking;
        private DateTime _startTime;
        private TimeSpan _summaryTime;

        public TimeSpan SummaryTime
        {
            get
            {
                if (_isWorking)
                {
                    throw new Exception(WasNotStopped);
                }
                return _summaryTime;
            }
        }

        public TimeSpan CurrentTime => DateTime.UtcNow - _startTime;

        public void Start()
        {
            if (_isWorking)
            {
                throw new Exception(WasNotStopped);
            }

            _isWorking = true;
            _summaryTime = default(TimeSpan);
            _startTime = DateTime.UtcNow;
        }

        public void Stop()
        {
            if (!_isWorking)
            {
                return;
            }

            _summaryTime = CurrentTime;
            _startTime = default(DateTime);
            _isWorking = false;
        }

        public void Reset()
        {
            _isWorking = false;
            _startTime = default(DateTime);
            _summaryTime = default(TimeSpan);
        }
    }
}
