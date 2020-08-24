// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.Common.Tests.Extensions.EnumerableExtensionsTests
{
    public class EnumerableExtensionsDataProvider
    {
        public static IEnumerable<object[]> ChunkifyData
        {
            get
            {
                yield return new object[]
                {
                    new List<int>() { 1, 2, 3, 4 },
                    1,
                    new List<int[]>() { new int[] { 1 }, new int[] { 2 }, new int[] { 3 }, new int[] { 4 } }
                };
                yield return new object[]
                {
                    new List<int>() { 1, 2, 3, 4 },
                    2,
                    new List<int[]>() { new int[] { 1, 2 }, new int[] { 3, 4 } }
                };
                yield return new object[]
                {
                    new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 },
                    3,
                    new List<int[]>() { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 7, 8 } }
                };
                yield return new object[]
                {
                    new List<int>() { 1, 2, 3, 4 },
                    10,
                    new List<int[]>() { new int[] { 1, 2, 3, 4 } }
                };
            }
        }
    }
}
