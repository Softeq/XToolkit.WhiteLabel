// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using NSubstitute;
using Softeq.XToolkit.Remote.Api;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Api
{
    public class RefitApiServiceFactoryTests
    {
        private readonly RefitApiServiceFactory _factory;

        public RefitApiServiceFactoryTests()
        {
            _factory = new RefitApiServiceFactory();
        }

        [Fact]
        public void CreateService_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.CreateService<IStubApiInterface>(null!);
            });
        }

        [Fact]
        public void CreateService_HttpClient_ThrowsArgumentNullException()
        {
            var httpClient = Substitute.For<HttpClient>();

            var result = _factory.CreateService<IStubApiInterface>(httpClient);

            Assert.IsAssignableFrom<IStubApiInterface>(result);
        }
    }
}
