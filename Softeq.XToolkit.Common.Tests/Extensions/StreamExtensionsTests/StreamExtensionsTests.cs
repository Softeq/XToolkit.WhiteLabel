// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using NSubstitute;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.StreamExtensionsTests
{
    public class StreamExtensionsTests
    {
        [Fact]
        public void ToArray_Null_ThrowsArgumentNullException()
        {
            Stream stream = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                stream.ToArray();
            });
        }

        [Fact]
        public void ToArray_EmptyMemoryStream_ReturnsEmptyArray()
        {
            Stream stream = new MemoryStream();

            var array = stream.ToArray();

            Assert.Empty(array);
        }

        [Fact]
        public void ToArray_MemoryStream_ReturnsArray()
        {
            var buffer = new byte[] { 1, 2, 3, 4, 5 };
            Stream stream = new MemoryStream(buffer);

            var array = stream.ToArray();

            Assert.Equal(buffer.Length, array.Length);
        }

        [Fact]
        public void ToArray_MemoryStreamWithPosition_ReturnsArray()
        {
            var buffer = new byte[] { 1, 2, 3, 4, 5 };
            Stream stream = new MemoryStream(buffer)
            {
                Position = 3
            };

            var array = stream.ToArray();

            Assert.Equal(buffer.Length, array.Length);
        }

        [Fact]
        public void ToArray_MemoryStream_UsesSystemMethod()
        {
            var stream = Substitute.For<MemoryStream>();

            var _ = ((Stream) stream).ToArray();

            stream.Received(1).ToArray();
        }

        [Fact]
        public void ToArray_EmptyFileStream_ReturnsEmptyArray()
        {
            Stream stream = new FileStream("TestFileName", FileMode.OpenOrCreate);

            var array = stream.ToArray();

            Assert.Empty(array);
        }
    }
}
