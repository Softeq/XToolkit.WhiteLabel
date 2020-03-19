// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.Bindings.Tests.BindingFactoryBaseTests
{
    internal class BindingFactoryBaseDataProvider
    {
        private const string CommandParameter = "TEST_PARAMETER";

        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] { true, CommandParameter };
                yield return new object[] { false, CommandParameter };
                yield return new object[] { true, null };
                yield return new object[] { false, null };
            }
        }
    }
}
