// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Helpers;
using Xunit;

#pragma warning disable 618

namespace Softeq.XToolkit.Common.Tests.Helpers.HashHelperTests
{
    public class HashHelperTests
    {
        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash2ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_NullArguments_DoesNotThrow(object arg1, object arg2)
        {
            HashHelper.GetHashCode(null, arg1, arg2);
        }

        [Theory]
        [InlineData(null, null)]
        public void GetHashCode_AllNullArguments_Persists(object arg1, object arg2)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2);
            var hash2 = HashHelper.GetHashCode(arg1, arg2);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash2ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_2Arguments_Persists(object arg1, object arg2)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2);
            var hash2 = HashHelper.GetHashCode(arg1, arg2);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash2ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_2Arguments_ReverseOrderDiffers(object arg1, object arg2)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2);
            var hash2 = HashHelper.GetHashCode(arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash3ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_3Arguments_Persists(object arg1, object arg2, object arg3)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3);
            var hash2 = HashHelper.GetHashCode(arg1, arg2, arg3);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash3ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_3Arguments_ReverseOrderDiffers(object arg1, object arg2, object arg3)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3);
            var hash2 = HashHelper.GetHashCode(arg3, arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash4ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_4Arguments_Persists(object arg1, object arg2, object arg3, object arg4)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4);
            var hash2 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash4ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_4Arguments_ReverseOrderDiffers(object arg1, object arg2, object arg3, object arg4)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4);
            var hash2 = HashHelper.GetHashCode(arg4, arg3, arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash5ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_5Arguments_Persists(object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5);
            var hash2 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash5ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_5Arguments_ReverseOrderDiffers(object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5);
            var hash2 = HashHelper.GetHashCode(arg5, arg4, arg3, arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash6ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_6Arguments_Persists(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6);
            var hash2 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash6ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_6Arguments_ReverseOrderDiffers(object arg1, object arg2, object arg3, object arg4, object arg5,
            object arg6)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6);
            var hash2 = HashHelper.GetHashCode(arg6, arg5, arg4, arg3, arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash7ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_7Arguments_Persists(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6,
            object arg7)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            var hash2 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash7ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_7Arguments_ReverseOrderDiffers(object arg1, object arg2, object arg3, object arg4, object arg5,
            object arg6, object arg7)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            var hash2 = HashHelper.GetHashCode(arg7, arg6, arg5, arg4, arg3, arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash8ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_8Arguments_Persists(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6,
            object arg7, object arg8)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            var hash2 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash8ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_8Arguments_ReverseOrderDiffers(object arg1, object arg2, object arg3, object arg4, object arg5,
            object arg6, object arg7, object arg8)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            var hash2 = HashHelper.GetHashCode(arg8, arg7, arg6, arg5, arg4, arg3, arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash9ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_9Arguments_Persists(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6,
            object arg7, object arg8, object arg9)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            var hash2 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash9ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_9Arguments_ReverseOrderDiffers(object arg1, object arg2, object arg3, object arg4, object arg5,
            object arg6, object arg7, object arg8, object arg9)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            var hash2 = HashHelper.GetHashCode(arg9, arg8, arg7, arg6, arg5, arg4, arg3, arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash10ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_10Arguments_Persists(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6,
            object arg7, object arg8, object arg9, object arg10)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            var hash2 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.Hash10ArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void GetHashCode_10Arguments_ReverseOrderDiffers(object arg1, object arg2, object arg3, object arg4, object arg5,
            object arg6, object arg7, object arg8, object arg9, object arg10)
        {
            var hash1 = HashHelper.GetHashCode(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            var hash2 = HashHelper.GetHashCode(arg10, arg9, arg8, arg7, arg6, arg5, arg4, arg3, arg2, arg1);

            Assert.NotEqual(hash1, hash2);
        }
    }
}
