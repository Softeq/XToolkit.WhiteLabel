// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Tests.Helpers.HashHelperTests
{
    internal static class HashHelperDataProvider
    {
        public static IEnumerable<object[]> Hash2ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1' }; // char
                yield return new object[] { "abc", "def" }; // string
                yield return new object[] { (byte) 0, (byte) 1 }; // byte
                yield return new object[] { 0, 1 }; // int
                yield return new object[] { 0.5f, 1.5f }; // float
                yield return new object[] { 0.1, 0.2 }; // double
                yield return new object[] { true, false }; // bool
                yield return new object[] { DateTime.MinValue, DateTime.MaxValue }; // DateTime
                yield return new object[] { new TestHashObject(0), new TestHashObject(1) }; // custom type
                yield return new object[] { '0', "abc" }; // mixed
            }
        }

        public static IEnumerable<object[]> Hash3ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1', '2' }; // char
                yield return new object[] { "abc", "def", "ghi" }; // string
                yield return new object[] { (byte) 0, (byte) 0, (byte) 1 }; // byte
                yield return new object[] { 0, 1, 2 }; // int
                yield return new object[] { 0.5f, 1.5f, 2.5f }; // float
                yield return new object[] { 0.1, 0.2, 0.3 }; // double
                yield return new object[] { true, true, false }; // bool
                yield return new object[] { DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue }; // DateTime
                yield return new object[] { new TestHashObject(0), new TestHashObject(1), new TestHashObject(2) }; // custom type
                yield return new object[] { '0', "abc", (byte) 0 }; // mixed
            }
        }

        public static IEnumerable<object[]> Hash4ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1', '2', '3' }; // char
                yield return new object[] { "abc", "def", "ghi", "jkl" }; // string
                yield return new object[] { (byte) 0, (byte) 1, (byte) 0, (byte) 1 }; // byte
                yield return new object[] { 0, 1, 2, 3 }; // int
                yield return new object[] { 0.5f, 1.5f, 2.5f, 3.5f }; // float
                yield return new object[] { 0.1, 0.2, 0.3, 0.4 }; // double
                yield return new object[] { true, false, true, false }; // bool
                yield return new object[]
                {
                    DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue
                }; // DateTime
                yield return new object[]
                {
                    new TestHashObject(0), new TestHashObject(1), new TestHashObject(2), new TestHashObject(3)
                }; // custom type
                yield return new object[] { '0', "abc", (byte) 0, 0 }; // mixed
            }
        }

        public static IEnumerable<object[]> Hash5ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1', '2', '3', '4' }; // char
                yield return new object[] { "abc", "def", "ghi", "jkl", "abc" }; // string
                yield return new object[] { (byte) 0, (byte) 0, (byte) 1, (byte) 1, (byte) 0 }; // byte
                yield return new object[] { 0, 1, 2, 3, 4 }; // int
                yield return new object[] { 0.5f, 1.5f, 2.5f, 3.5f, 4.5f }; // float
                yield return new object[] { 0.1, 0.2, 0.3, 0.4, 0.5 }; // double
                yield return new object[] { true, true, true, false, false }; // bool
                yield return new object[]
                {
                    DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue
                }; // DateTime
                yield return new object[]
                {
                    new TestHashObject(0), new TestHashObject(1), new TestHashObject(2), new TestHashObject(3),
                    new TestHashObject(4)
                }; // custom type
                yield return new object[] { '0', "abc", (byte) 0, 0, 0.5f }; // mixed
            }
        }

        public static IEnumerable<object[]> Hash6ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1', '2', '3', '4', '0' }; // char
                yield return new object[] { "abc", "def", "ghi", "jkl", "abc", "def" }; // string
                yield return new object[] { (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 0, (byte) 1 }; // byte
                yield return new object[] { 0, 1, 2, 3, 4, 5 }; // int
                yield return new object[] { 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f }; // float
                yield return new object[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6 }; // double
                yield return new object[] { true, false, true, false, true, false }; // bool
                yield return new object[]
                {
                    DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue,
                    DateTime.MaxValue
                }; // DateTime
                yield return new object[]
                {
                    new TestHashObject(0), new TestHashObject(1), new TestHashObject(2), new TestHashObject(3),
                    new TestHashObject(4), new TestHashObject(5)
                }; // custom type
                yield return new object[] { '0', "abc", (byte) 0, 0, 0.5f, 0.1 }; // mixed
            }
        }

        public static IEnumerable<object[]> Hash7ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1', '2', '3', '4', '0', '1' }; // char
                yield return new object[] { "abc", "def", "ghi", "jkl", "abc", "def", "ghi" }; // string
                yield return new object[] { (byte) 0, (byte) 0, (byte) 0, (byte) 1, (byte) 1, (byte) 1, (byte) 0 }; // byte
                yield return new object[] { 0, 1, 2, 3, 4, 5, 6 }; // int
                yield return new object[] { 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f }; // float
                yield return new object[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7 }; // double
                yield return new object[] { true, true, true, false, true, false, true }; // bool
                yield return new object[]
                {
                    DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue,
                    DateTime.MaxValue, DateTime.MaxValue
                }; // DateTime
                yield return new object[]
                {
                    new TestHashObject(0), new TestHashObject(1), new TestHashObject(2), new TestHashObject(3),
                    new TestHashObject(4), new TestHashObject(5), new TestHashObject(6)
                }; // custom type
                yield return new object[] { '0', "abc", (byte) 0, 0, 0.5f, 0.1, true }; // mixed
            }
        }

        public static IEnumerable<object[]> Hash8ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1', '2', '3', '4', '0', '1', '2' }; // char
                yield return new object[] { "abc", "def", "ghi", "jkl", "abc", "def", "ghi", "jkl" }; // string
                yield return new object[]
                {
                    (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 0, (byte) 1
                }; // byte
                yield return new object[] { 0, 1, 2, 3, 4, 5, 6, 7 }; // int
                yield return new object[] { 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f, 7.5f }; // float
                yield return new object[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8 }; // double
                yield return new object[] { true, false, true, false, true, false, true, false }; // bool
                yield return new object[]
                {
                    DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue,
                    DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue
                }; // DateTime
                yield return new object[]
                {
                    new TestHashObject(0), new TestHashObject(1), new TestHashObject(2), new TestHashObject(3),
                    new TestHashObject(4), new TestHashObject(5), new TestHashObject(6), new TestHashObject(7)
                }; // custom type
                yield return new object[] { '0', "abc", (byte) 0, 0, 0.5f, 0.1, true, DateTime.MinValue }; // mixed
            }
        }

        public static IEnumerable<object[]> Hash9ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1', '2', '3', '4', '0', '1', '2', '3' }; // char
                yield return new object[] { "abc", "def", "ghi", "jkl", "abc", "def", "ghi", "jkl", "abc" }; // string
                yield return new object[]
                {
                    (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 1
                }; // byte
                yield return new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }; // int
                yield return new object[] { 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f, 7.5f, 8.5f }; // float
                yield return new object[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 }; // double
                yield return new object[] { true, false, true, false, true, false, true, false, false }; // bool
                yield return new object[]
                {
                    DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue,
                    DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue, DateTime.MaxValue
                }; // DateTime
                yield return new object[]
                {
                    new TestHashObject(0), new TestHashObject(1), new TestHashObject(2), new TestHashObject(3),
                    new TestHashObject(4), new TestHashObject(5), new TestHashObject(6), new TestHashObject(7),
                    new TestHashObject(8)
                }; // custom type
                yield return new object[]
                {
                    '0', "abc", (byte) 0, 0, 0.5f, 0.1, true, DateTime.MinValue, new TestHashObject(0)
                }; // mixed
            }
        }

        public static IEnumerable<object[]> Hash10ArgumentsData
        {
            get
            {
                yield return new object[] { '0', '1', '2', '3', '4', '0', '1', '2', '3', '4' }; // char
                yield return new object[] { "abc", "def", "ghi", "jkl", "abc", "def", "ghi", "jkl", "abc", "def" }; // string
                yield return new object[]
                {
                    (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 0, (byte) 1, (byte) 0, (byte) 1
                }; // byte
                yield return new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // int
                yield return new object[] { 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f, 7.5f, 8.5f, 9.5f }; // float
                yield return new object[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 }; // double
                yield return new object[] { true, false, true, false, true, false, true, false, true, false }; // bool
                yield return new object[]
                {
                    DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue,
                    DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue, DateTime.MinValue, DateTime.MaxValue
                }; // DateTime
                yield return new object[]
                {
                    new TestHashObject(0), new TestHashObject(1), new TestHashObject(2), new TestHashObject(3),
                    new TestHashObject(4), new TestHashObject(5), new TestHashObject(6), new TestHashObject(7),
                    new TestHashObject(8), new TestHashObject(9)
                }; // custom type
                yield return new object[]
                {
                    '0', "abc", (byte) 0, 0, 0.5f, 0.1, true, DateTime.MinValue, new TestHashObject(0), new TestHashObject(1)
                }; // mixed
            }
        }

        public static IEnumerable<object[]> HashValueTypeArgumentsData
        {
            get { yield return new object[] { 1, 1.0, 1f, DateTime.MinValue, '1', true }; }
        }

        public static IEnumerable<object[]> HashNullableValueTypeArgumentsData
        {
            get
            {
                yield return new object[] { null, 1.0, 1f, DateTime.MinValue, '1', true };
                yield return new object[] { 1, null, 1f, DateTime.MinValue, '1', true };
                yield return new object[] { 1, 1.0, null, DateTime.MinValue, '1', true };
                yield return new object[] { 1, 1.0, 1f, null, '1', true };
                yield return new object[] { 1, 1.0, 1f, DateTime.MinValue, null, true };
                yield return new object[] { 1, 1.0, 1f, DateTime.MinValue, '1', null };
                yield return new object[] { null, null, null, null, null, null };
            }
        }

        public static IEnumerable<object[]> HashReferenceTypeArgumentsData
        {
            get
            {
                yield return new object[] {"42", new object(), new object[]{}, new TestHashObject(0)};
                yield return new object[] {null, new object(), new object[]{}, new TestHashObject(0)};
                yield return new object[] {"42", null, new object[]{}, new TestHashObject(0)};
                yield return new object[] {"42", new object(), null, new TestHashObject(0)};
                yield return new object[] {"42", new object(), new object[]{}, null};
                yield return new object[] {null, null, null, null};
            }
        }
    }
}
