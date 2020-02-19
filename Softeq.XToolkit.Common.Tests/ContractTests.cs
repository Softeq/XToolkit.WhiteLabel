// Developed by Softeq Development Corporation
// http://www.softeq.com

using NSubstitute;
using Softeq.XToolkit.Common.Interfaces;
using Xunit;

#nullable disable

namespace Softeq.XToolkit.Common.Tests
{
    public class ContractTests
    {
        [Fact]
        public void IInternalSettingsTest()
        {
            var mock = Substitute.For<IInternalSettings>();
            mock.AddOrUpdateValue("key", 1);
            mock.Clear();
            mock.Contains("key");
            mock.GetValueOrDefault("key", 1);
            mock.Remove("key");
        }
    }
}
