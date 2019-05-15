// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Helpers;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common
{
    public class HelpersTests
    {
        [Fact]
        public void HashHelperTest()
        {
            Assert.Equal(HashHelper.GetHashCode("0", "1"), HashHelper.GetHashCode("0", "1"));
            Assert.Equal(HashHelper.GetHashCode("0", "1", "2"), HashHelper.GetHashCode("0", "1", "2"));
            Assert.Equal(HashHelper.GetHashCode("0", "1", "2", "3"), HashHelper.GetHashCode("0", "1", "2", "3"));
            Assert.Equal(HashHelper.GetHashCode("0", "1", "2", "3", "4"),
                HashHelper.GetHashCode("0", "1", "2", "3", "4"));
            Assert.Equal(HashHelper.GetHashCode("0", "1", "2", "3", "4", "5"),
                HashHelper.GetHashCode("0", "1", "2", "3", "4", "5"));
            Assert.Equal(HashHelper.GetHashCode("0", "1", "2", "3", "4", "5", "6"),
                HashHelper.GetHashCode("0", "1", "2", "3", "4", "5", "6"));
            Assert.Equal(HashHelper.GetHashCode("0", "1", "2", "3", "4", "5", "6", "7"),
                HashHelper.GetHashCode("0", "1", "2", "3", "4", "5", "6", "7"));
            Assert.Equal(HashHelper.GetHashCode("0", "1", "2", "3", "4", "5", "6", "7", "8"),
                HashHelper.GetHashCode("0", "1", "2", "3", "4", "5", "6", "7", "8"));
            Assert.Equal(HashHelper.GetHashCode("0", "1", "2", "3", "4", "5", "6", "7", "8", "9"),
                HashHelper.GetHashCode("0", "1", "2", "3", "4", "5", "6", "7", "8", "9"));
        }

        [Fact]
        public void StringsHelperTest()
        {
            Assert.Equal("One", StringsHelper.CapitalizeFirstLetter("one"));
            Assert.Equal("TWO", StringsHelper.CapitalizeFirstLetter("tWO"));
            Assert.Equal("Three", StringsHelper.CapitalizeFirstLetter("Three"));
        }
    }
}