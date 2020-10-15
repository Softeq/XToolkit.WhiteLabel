// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Remote.Exceptions;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Exceptions
{
    public class ExpiredRefreshTokenExceptionTests
    {
        [Fact]
        public void Ctor_Null_ReturnsWithNullInnerException()
        {
            var obj = new ExpiredRefreshTokenException(null!);

            Assert.Null(obj.InnerException);
        }

        [Fact]
        public void Ctor_Exception_ReturnsInnerException()
        {
            var exception = new Exception();

            var obj = new ExpiredRefreshTokenException(exception);

            Assert.Same(exception, obj.InnerException);
        }

        [Fact]
        public void Ctor_Exception_ReturnsNotEmptyMessage()
        {
            var exception = Substitute.For<Exception>();

            var obj = new ExpiredRefreshTokenException(exception);

            Assert.NotEmpty(obj.Message);
        }
    }
}
