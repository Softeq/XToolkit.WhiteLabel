// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.ComponentModel;

namespace Softeq.XToolkit.Common.Tests.Extensions.EnumExtensionsTests
{
    public enum TestEnum
    {
        Value1,
        Value2,
        Value3 = 42,
        [DescriptionAttribute("value number 4")]
        Value4 = 4,
        [DescriptionAttribute("Description")]
        Value5
    }
}
