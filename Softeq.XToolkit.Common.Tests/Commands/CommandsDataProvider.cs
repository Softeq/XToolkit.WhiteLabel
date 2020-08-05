// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Text;

namespace Softeq.XToolkit.Common.Tests.Commands
{
    public static class CommandsDataProvider
    {
        public const string DefaultParameter = "CanExecute";

        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] { DefaultParameter, true };
                yield return new object[] { 123, false };
                yield return new object[] { null, false };
            }
        }

        public static IEnumerable<object[]> Parameters
        {
            get
            {
                yield return new object[] { DefaultParameter };
                yield return new object[] { null };
            }
        }

        public static bool CanExecuteWhenNotNull(string parameter)
        {
            return parameter != null;
        }

        public static bool CanExecuteWhenPositive(int parameter)
        {
            return parameter > 0;
        }
    }
}
