// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Helpers.Hashing;
using Softeq.XToolkit.Common.Tests.Helpers.HashHelperTests;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Helpers.Hashing
{
    public class HashCalculatorTests
    {
        [Theory]
        [MemberData(nameof(HashHelperDataProvider.HashValueTypeArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void CalculateHashCode_ValueTypes_SameValue(int arg1, double arg2, float arg3, DateTime arg4, char arg5, bool arg6)
        {
            var hash1 = Hash.Get()
                .Using(arg1)
                .Using(arg2)
                .Using(arg3)
                .Using(arg4)
                .Using(arg5)
                .Using(arg6)
                .Calculate();
            var hash2 = Hash.Get()
                .Using(arg1)
                .Using(arg2)
                .Using(arg3)
                .Using(arg4)
                .Using(arg5)
                .Using(arg6)
                .Calculate();

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.HashNullableValueTypeArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void CalculateHashCode_NullableValueTypes_SameValue(int? arg1, double? arg2, float? arg3, DateTime? arg4, char? arg5, bool? arg6)
        {
            var hash1 = Hash.Get()
                .Using(arg1)
                .Using(arg2)
                .Using(arg3)
                .Using(arg4)
                .Using(arg5)
                .Using(arg6)
                .Calculate();
            var hash2 = Hash.Get()
                .Using(arg1)
                .Using(arg2)
                .Using(arg3)
                .Using(arg4)
                .Using(arg5)
                .Using(arg6)
                .Calculate();

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [MemberData(nameof(HashHelperDataProvider.HashReferenceTypeArgumentsData), MemberType = typeof(HashHelperDataProvider))]
        public void CalculateHashCode_ReferenceTypes_SameValue(string arg1, object arg2, object[] arg3, TestHashObject arg4)
        {
            var hash1 = Hash.Get()
                .Using(arg1)
                .Using(arg2)
                .Using(arg3)
                .Using(arg4)
                .Calculate();
            var hash2 = Hash.Get()
                .Using(arg1)
                .Using(arg2)
                .Using(arg3)
                .Using(arg4)
                .Calculate();

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [InlineData(42, null, "42", null)]
        public void CalculateHashCode_MixedTypes_SameValue(int arg1, int? arg2, string arg3, object arg4)
        {
            var hash1 = Hash.Get()
                .Using(arg1)
                .Using(arg2)
                .Using(arg3)
                .Using(arg4)
                .Calculate();
            var hash2 = Hash.Get()
                .Using(arg1)
                .Using(arg2)
                .Using(arg3)
                .Using(arg4)
                .Calculate();

            Assert.Equal(hash1, hash2);
        }
    }
}
