// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Disposables;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Disposables
{
    public class DisposableTests
    {
        [Fact]
        public void Create()
        {
            var instance = Disposable.Create(() => { });
            Assert.True(instance is IDisposable);
        }

        [Fact]
        public void Create_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => Disposable.Create(null!));
        }

        [Fact]
        public void IsDisposed_False()
        {
            var instance = new AnonymousDisposable(() => { });
            Assert.False(instance.IsDisposed);
        }

        [Fact]
        public void Dispose()
        {
            var instance = new AnonymousDisposable(() => { });
            using (instance)
            {
            }

            Assert.True(instance.IsDisposed);
        }
    }
}
