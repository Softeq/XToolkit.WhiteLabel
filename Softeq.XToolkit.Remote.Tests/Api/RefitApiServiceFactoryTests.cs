// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using NSubstitute;
using Refit;
using Softeq.XToolkit.Remote.Api;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Api
{
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue", Justification = "Need for tests")]
    public class RefitApiServiceFactoryTests
    {
        private readonly RefitApiServiceFactory _factory;

        public RefitApiServiceFactoryTests()
        {
            _factory = new RefitApiServiceFactory();
        }

        [Fact]
        public void Ctor_Default_CreatesIApiServiceFactory()
        {
            Assert.IsAssignableFrom<IApiServiceFactory>(_factory);
        }

        [Fact]
        public void Ctor_Null_CreatesIApiServiceFactory()
        {
            var factory= new RefitApiServiceFactory(null);

            Assert.IsAssignableFrom<IApiServiceFactory>(factory);
        }

        [Fact]
        public void Ctor_RefitSettings_CreatesIApiServiceFactory()
        {
            var refitSettings = new RefitSettings();

            var factory = new RefitApiServiceFactory(refitSettings);

            Assert.IsAssignableFrom<IApiServiceFactory>(factory);
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
        public void CreateService_HttpClient_ReturnsCorrectInstance()
        {
            var httpClient = Substitute.For<HttpClient>();

            var result = _factory.CreateService<IStubApiInterface>(httpClient);

            Assert.IsAssignableFrom<IStubApiInterface>(result);
        }
    }
}
