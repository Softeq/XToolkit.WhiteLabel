// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using NSubstitute;
using Softeq.XToolkit.Remote.Api;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests
{
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue", Justification = "Need for tests")]
    public class RemoteServiceFactoryTests
    {
        private readonly RemoteServiceFactory _factory;

        public RemoteServiceFactoryTests()
        {
            _factory = new RemoteServiceFactory();
        }

        [Fact]
        public void Ctor_Default_CreatesIRemoteServiceFactory()
        {
            var factory = new RemoteServiceFactory();

            Assert.IsAssignableFrom<IRemoteServiceFactory>(factory);
        }

        [Fact]
        public void Ctor_Null_CreatesIRemoteServiceFactory()
        {
            var factory = new RemoteServiceFactory(null);

            Assert.IsAssignableFrom<IRemoteServiceFactory>(factory);
        }

        [Fact]
        public void Ctor_ApiServiceFactory_CreatesIRemoteServiceFactory()
        {
            var apiServiceFactory = Substitute.For<IApiServiceFactory>();

            var factory = new RemoteServiceFactory(apiServiceFactory);

            Assert.IsAssignableFrom<IRemoteServiceFactory>(factory);
        }

        [Fact]
        public void Create_NullBaseUrl_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Create<IStubApiInterface>((null as string)!);
            });
        }

        [Fact]
        public void Create_BaseUrl_ReturnsRemoteService()
        {
            var result = _factory.Create<IStubApiInterface>("https://softeq.com");

            Assert.IsAssignableFrom<IRemoteService<IStubApiInterface>>(result);
        }

        [Fact]
        public void Create_NullHttpClient_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Create<IStubApiInterface>((null as HttpClient)!);
            });
        }

        [Fact]
        public void Create_HttpClient_ReturnsRemoteService()
        {
            var httpClient = Substitute.For<HttpClient>();

            var result = _factory.Create<IStubApiInterface>(httpClient);

            Assert.IsAssignableFrom<IRemoteService<IStubApiInterface>>(result);
        }
    }
}
