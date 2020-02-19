// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections;
using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Tests.Logger
{
    public class ConsoleLoggerMessageTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "LogCategory", "LogMessage" };
            yield return new object[] { "LogCategory", null };
            yield return new object[] { null, "LogMessage" };
            yield return new object[] { null, null };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
