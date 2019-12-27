// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections;
using System.Collections.Generic;
using System.Text;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Common.Tests.Collections.BiDictionaryTests
{
    public class TestData
    {
        public BiDictionary<int, string> Dictionary { get; }

        public TestData()
        {
            Dictionary = new BiDictionary<int, string>();
        }

        public TestData(int count)
        {
            Dictionary = new BiDictionary<int, string>();
            for (int i = 0; i < count; i++)
            {
                Dictionary.Add(i, i.ToString());
            }
        }

        public IDictionary DictionaryInterface
        {
            get => Dictionary;
        }

        public ICollection<KeyValuePair<int, string>> CollectionInterface
        {
            get => Dictionary;
        }

        public string ExtractResult()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(",", Dictionary);
            sb.Append("-");
            sb.AppendJoin(",", Dictionary.Reverse);
            return sb.ToString();
        }
    }

    public class ReverseTestData
    {
        public BiDictionary<string, int> Dictionary { get; }

        public ReverseTestData()
        {
            Dictionary = new BiDictionary<string, int>();
        }

        public ReverseTestData(int count)
        {
            Dictionary = new BiDictionary<string, int>();
            for (int i = 0; i < count; i++)
            {
                Dictionary.Add(i.ToString(), i);
            }
        }

        public IDictionary ReverseDictionaryInterface
        {
            get => (IDictionary) Dictionary.Reverse;
        }

        public ICollection<KeyValuePair<int, string>> ReverseCollectionInterface
        {
            get => Dictionary.Reverse;
        }

        public string ExtractResult()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(",", Dictionary);
            sb.Append("-");
            sb.AppendJoin(",", Dictionary.Reverse);
            return sb.ToString();
        }
    }
}
