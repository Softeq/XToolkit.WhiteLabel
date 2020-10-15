// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(string message)
            : base(message)
        {
        }

        public PropertyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
