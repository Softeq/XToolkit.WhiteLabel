// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Tests.Helpers;
using Softeq.XToolkit.Common.Weak;
using Xunit;

#nullable disable

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public class WeakReferenceExTest
    {
        private WeakReferenceEx<CommonTestClass> _reference;
        private CommonTestClass _target;

        private void Setup()
        {
            _target = new CommonTestClass();
            _reference = WeakReferenceEx.Create(_target);
        }

        [Fact]
        public void Test()
        {
            Setup();

            Assert.NotNull(_target);
            Assert.NotNull(_reference);

            _target = null;
            GC.Collect();

            Assert.Null(_reference.Target);
        }
    }
}
