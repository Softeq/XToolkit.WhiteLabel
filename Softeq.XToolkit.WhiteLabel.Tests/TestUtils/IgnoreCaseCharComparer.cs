// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.WhiteLabel.Tests.TestUtils
{
    public class IgnoreCaseCharComparer : IEqualityComparer<char>
    {
        public bool Equals(char x, char y)
        {
            return x.ToString().Equals(
                y.ToString(),
                StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(char obj) => obj.GetHashCode();
    }
}
