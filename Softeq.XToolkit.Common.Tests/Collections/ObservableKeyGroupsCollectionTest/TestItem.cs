// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class TestItem<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        public TKey Key { get; }
        public TValue Value { get; }
        public int Index { get; }

        public bool IsKeySelectedFire { get; private set; }
        public bool IsValueSelectedFire { get; private set; }
        public bool IsIndexSelectedFire { get; private set; }

        public TestItem(TKey key, TValue value, int index = 0)
        {
            Key = key;
            Value = value;
            Index = index;
        }

        public TKey SelectKey()
        {
            IsKeySelectedFire = true;

            return Key;
        }

        public TValue SelectValue()
        {
            IsValueSelectedFire = true;

            return Value;
        }

        public int SelectIndex()
        {
            IsIndexSelectedFire = true;

            return Index;
        }
    }
}