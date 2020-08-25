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
        public void Create_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Disposable.Create(null!));
        }

        [Fact]
        public void Dispose()
        {
            var disposed = false;
            var d = Disposable.Create(() => { disposed = true; });

            Assert.False(disposed);

            d.Dispose();

            Assert.True(disposed);
        }
    }
}
