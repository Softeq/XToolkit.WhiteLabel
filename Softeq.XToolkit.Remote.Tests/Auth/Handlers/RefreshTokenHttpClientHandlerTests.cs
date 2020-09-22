// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.Remote.Auth.Handlers;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers
{
    public class RefreshTokenHttpClientHandlerTests
    {
        [Fact]
        public void Ctor_NullAccessToken_ThrowsArgumentNullException()
        {
            var getRefreshToken = Substitute.For<Func<Task>>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new RefreshTokenHttpClientHandler(null!, getRefreshToken);
            });
        }

        [Fact]
        public void Ctor_NullRefreshToken_ThrowsArgumentNullException()
        {
            var getAccessToken = Substitute.For<Func<Task<string>>>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new RefreshTokenHttpClientHandler(getAccessToken, null!);
            });
        }
    }
}
