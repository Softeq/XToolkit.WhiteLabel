// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Runtime.CompilerServices;

namespace Softeq.XToolkit.Common.Tests
{
    internal class CommonTestException : Exception
    {
        public CommonTestException([CallerMemberName] string callerName = "")
            : base($"Test exception from {callerName}")
        {
        }
    }
}
