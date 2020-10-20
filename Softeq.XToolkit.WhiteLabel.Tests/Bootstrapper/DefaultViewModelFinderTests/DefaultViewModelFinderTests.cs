// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Bootstrapper.DefaultViewModelFinderTests
{
    public class DefaultViewModelFinderTests
    {
        [Fact]
        public void NewObject_EmptyCtor_Creates()
        {
            var obj = new DefaultViewModelFinder();

            Assert.IsAssignableFrom<IViewModelFinder>(obj);
        }

        [Fact]
        public void NewObject_NotEmptyCtor_Creates()
        {
            var obj = new DefaultViewModelFinder(typeof(Assert));

            Assert.IsAssignableFrom<IViewModelFinder>(obj);
        }

        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(CustomViewControllerStub), true)]
        [InlineData(typeof(CustomActivityStub), true)]
        [InlineData(typeof(CustomViewStub), true)]
        [InlineData(typeof(VmActivityStub), true)]
        [InlineData(typeof(VmViewControllerStub), true)]
        public void IsViewType_CorrectType_ReturnsExpectedResult(Type type, bool expectedResult)
        {
            var obj = new DefaultViewModelFinder(
                typeof(ViewControllerStub),
                typeof(ActivityStub),
                typeof(IDroidViewStub));

            var result = obj.IsViewType(type);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsViewType_CheckNullTypeForEmptyViewTypes_ReturnsFalse()
        {
            var obj = new DefaultViewModelFinder();

            var result = obj.IsViewType(null);

            Assert.False(result);
        }

        [Fact]
        public void IsViewType_CheckNullType_ReturnsFalse()
        {
            var obj = new DefaultViewModelFinder(typeof(Assert));

            var result = obj.IsViewType(null);

            Assert.False(result);
        }

        [Fact]
        public void GetViewModelToViewMapping_NullAssembliesList_ThrowsArgumentNullException()
        {
            var obj = new DefaultViewModelFinder();

            Assert.Throws<ArgumentNullException>(() => obj.GetViewModelToViewMapping(null));
        }

        [Fact]
        public void GetViewModelToViewMapping_EmptyAssembliesList_ReturnsEmptyMap()
        {
            var obj = new DefaultViewModelFinder();

            var result = obj.GetViewModelToViewMapping(Enumerable.Empty<Assembly>());

            Assert.Empty(result);
        }

        [Fact]
        public void GetViewModelToViewMapping_AssembliesList_ReturnsCorrectMap()
        {
            var obj = new DefaultViewModelFinder(typeof(ActivityStub), typeof(ViewControllerStub));
            var assemblies = new[]
            {
                typeof(DefaultViewModelFinder).Assembly,
                typeof(DefaultViewModelFinderTests).Assembly
            };

            var result = obj.GetViewModelToViewMapping(assemblies);

            Assert.NotEmpty(result);
            Assert.Equal(typeof(VmActivityStub), result[typeof(ViewModelStub1)]);
            Assert.Equal(typeof(VmViewControllerStub), result[typeof(ViewModelStub2)]);
        }
    }
}
