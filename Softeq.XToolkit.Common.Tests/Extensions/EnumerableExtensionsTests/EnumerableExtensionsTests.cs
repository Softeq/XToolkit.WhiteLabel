// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.EnumerableExtensionsTests
{
    public class EnumerableExtensionsTests
    {
        private readonly IEnumerable<int> _testEnumerable = new List<int>() { 1, 2, 3 };
        private readonly Action<int> _testAction = Substitute.For<Action<int>>();

        [Fact]
        public void EmptyIfNull_NullEnumerable_ReturnsEmptyEnumerable()
        {
            IEnumerable<int> enumerable = null;

            var result = enumerable.EmptyIfNull();

            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.NotEqual(enumerable, result);
        }

        [Fact]
        public void EmptyIfNull_NotNullEnumerable_ReturnsSameEnumerable()
        {
            var result = _testEnumerable.EmptyIfNull();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(_testEnumerable, result);
        }

        [Fact]
        public void Apply_NullEnumerableNullAction_ThrowsArgumentNullException()
        {
            IEnumerable<int> enumerable = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                enumerable.Apply(null);
            });
        }

        [Fact]
        public void Apply_NullEnumerableNotNullAction_ThrowsArgumentNullException()
        {
            IEnumerable<int> enumerable = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                enumerable.Apply(_testAction);
            });
        }

        [Fact]
        public void Apply_NotNullEnumerableNullAction_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _testEnumerable.Apply(null);
            });
        }

        [Fact]
        public void Apply_EmptyEnumerableNotNullAction_DoesNotThrow()
        {
            IEnumerable<int> enumerable = new List<int>();

            enumerable.Apply(_testAction);

            _testAction.DidNotReceive().Invoke(Arg.Any<int>());
        }

        [Fact]
        public void Apply_NotNullParams_ActionIsCalledForEveryElement()
        {
            _testAction.ClearReceivedCalls();

            _testEnumerable.Apply(_testAction);

            _testAction.ReceivedWithAnyArgs(_testEnumerable.Count()).Invoke(Arg.Any<int>());
        }
    }
}
