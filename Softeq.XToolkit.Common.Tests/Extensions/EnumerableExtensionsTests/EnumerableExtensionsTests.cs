// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
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
        public void Apply_NullEnumerableNullAction_ThrowsNullReferenceException()
        {
            IEnumerable<int> enumerable = null;

            Assert.Throws<NullReferenceException>(() =>
            {
                enumerable.Apply(null);
            });
        }

        [Fact]
        public void Apply_NullEnumerableNotNullAction_ThrowsNullReferenceException()
        {
            IEnumerable<int> enumerable = null;

            Assert.Throws<NullReferenceException>(() =>
            {
                enumerable.Apply(_testAction);
            });
        }

        [Fact]
        public void Apply_NotNullEnumerableNullAction_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() =>
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
            foreach (var item in _testEnumerable)
            {
                _testAction.Received().Invoke(item);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void Chunkify_NullEnumerable_ThrowsArgumentNullException(int size)
        {
            IEnumerable<int> enumerable = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                enumerable.Chunkify(size).ToList();
            });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Chunkify_IncorrectSize_ThrowsOutOfRangeException(int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _testEnumerable.Chunkify(size).ToList();
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void Chunkify_CorrectSize_ReturnsValidChunks(int size)
        {
            var expectedTotalCount = (int) Math.Ceiling(_testEnumerable.Count() / (double) size);
            var lastChunkCount = _testEnumerable.Count() % size;

            var result = _testEnumerable.Chunkify(size).ToList();

            Assert.Equal(expectedTotalCount, result.Count());

            var initialArray = _testEnumerable.ToArray();
            for (var i = 0; i < result.Count(); i++)
            {
                var chunk = result[i];
                var expectedSize = lastChunkCount != 0 && i == expectedTotalCount - 1 ? lastChunkCount : size;
                Assert.Equal(expectedSize, chunk.Count());
                for (int j = 0; j < chunk.Count(); j++)
                {
                    Assert.Equal(initialArray[(i * size) + j], chunk[j]);
                }
            }
        }
    }
}
