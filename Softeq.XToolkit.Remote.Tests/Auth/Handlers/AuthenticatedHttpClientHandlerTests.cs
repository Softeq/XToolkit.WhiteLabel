// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Remote.Auth.Handlers;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers
{
    public class AuthenticatedHttpClientHandlerTests
    {
        [Fact]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new AuthenticatedHttpClientHandler(null!);
            });
        }

    }
}
