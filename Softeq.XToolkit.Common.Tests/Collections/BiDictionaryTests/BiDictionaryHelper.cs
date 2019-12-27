// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections;
using System.Collections.Generic;
using System.Text;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Common.Tests.Collections.BiDictionaryTests
{
    public static class BiDictionaryHelper
    {
        public static BiDictionary<int, string> CreateEmpty()
        {
            return new BiDictionary<int, string>();
        }

        public static BiDictionary<int, string> CreateWithTwoItems()
        {
            return new BiDictionary<int, string> { { 0, "0" }, { 1, "1" } };
        }

        public static BiDictionary<string, int> CreateEmptyReverse()
        {
            return new BiDictionary<string, int>();
        }

        public static BiDictionary<string, int> CreateWithTwoItemsReverse()
        {
            return new BiDictionary<string, int> { { "0", 0 }, { "1", 1 } };
        }

        public static IDictionary ToIDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return (IDictionary) dictionary;
        }

        public static ICollection<KeyValuePair<TKey, TValue>> ToICollection<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary;
        }

        public static IEnumerable ToIEnumerable(this IDictionary dictionary)
        {
            return dictionary;
        }

        public static string GetResult<TKey, TValue>(this IDictionary dictionary)
        {
            var biDict = (BiDictionary<TKey, TValue>) dictionary;
            return biDict.GetResult();
        }

        public static string GetResult<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue>> dictionary)
        {
            var biDict = (BiDictionary<TKey, TValue>) dictionary;
            return biDict.GetResult();
        }

        public static string GetResult<TKey, TValue>(this BiDictionary<TKey, TValue> dictionary)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(",", dictionary);
            sb.Append("-");
            sb.AppendJoin(",", dictionary.Reverse);
            return sb.ToString();
        }
    }
}
