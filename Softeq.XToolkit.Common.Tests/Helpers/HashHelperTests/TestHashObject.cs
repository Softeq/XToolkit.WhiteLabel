﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit.Abstractions;

namespace Softeq.XToolkit.Common.Tests.Helpers.HashHelperTests
{
    internal class TestHashObject : IXunitSerializable
    {
        private static readonly string _key = $"{nameof(TestHashObject)}_{nameof(_value)}";
        private int _value;

        public TestHashObject()
        {
        }

        public TestHashObject(int value)
        {
            _value = value;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            _value = info.GetValue<int>(_key);
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(_key, _value);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return _value.GetHashCode() + nameof(TestHashObject).GetHashCode();
        }
    }
}
