// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Tests.Core.Common.Helpers;
using Softeq.XToolkit.Common;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common.WeakTests
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