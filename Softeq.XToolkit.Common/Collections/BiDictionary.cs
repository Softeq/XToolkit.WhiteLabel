// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace Softeq.XToolkit.Common.Collections
{
    [Serializable]
    public class BiDictionary<TFirst, TSecond> :
        IDictionary<TFirst, TSecond>, IReadOnlyDictionary<TFirst, TSecond>,
        IDictionary,
        ISerializable
    {
        private readonly IDictionary<TFirst, TSecond> _firstToSecond = new Dictionary<TFirst, TSecond>();

        [NonSerialized]
        private readonly ReverseDictionary _reverseDictionary;

        [NonSerialized]
        private readonly IDictionary<TSecond, TFirst> _secondToFirst;

        public BiDictionary()
        {
            _secondToFirst = new Dictionary<TSecond, TFirst>();
            _reverseDictionary = new ReverseDictionary(this);
        }

        protected BiDictionary(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            var data = (byte[]) info.GetValue("firstToSecond", typeof(byte[]));
            using (var memoryStream = new MemoryStream(data))
            {
                var binaryFormatter = new BinaryFormatter();
                _firstToSecond = (Dictionary<TFirst, TSecond>) binaryFormatter.Deserialize(memoryStream);
            }

            _secondToFirst = new Dictionary<TSecond, TFirst>();
            _reverseDictionary = new ReverseDictionary(this);
        }

        public IDictionary<TSecond, TFirst> Reverse => _reverseDictionary;

        object ICollection.SyncRoot => ((ICollection) _firstToSecond).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection) _firstToSecond).IsSynchronized;

        bool IDictionary.IsFixedSize => ((IDictionary) _firstToSecond).IsFixedSize;

        object IDictionary.this[object key]
        {
            get => ((IDictionary) _firstToSecond)[key];
            set
            {
                ((IDictionary) _firstToSecond)[key] = value;
                ((IDictionary) _secondToFirst)[value] = key;
            }
        }

        ICollection IDictionary.Keys => ((IDictionary) _firstToSecond).Keys;

        ICollection IDictionary.Values => ((IDictionary) _firstToSecond).Values;

        IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary) _firstToSecond).GetEnumerator();

        void IDictionary.Add(object key, object value)
        {
            ((IDictionary) _firstToSecond).Add(key, value);
            ((IDictionary) _secondToFirst).Add(value, key);
        }

        void IDictionary.Remove(object key)
        {
            var firstToSecond = (IDictionary) _firstToSecond;
            if (!firstToSecond.Contains(key))
            {
                return;
            }

            var value = firstToSecond[key];
            firstToSecond.Remove(key);
            ((IDictionary) _secondToFirst).Remove(value);
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary) _firstToSecond).Contains(key);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IDictionary) _firstToSecond).CopyTo(array, index);
        }

        public int Count => _firstToSecond.Count;

        public bool IsReadOnly => _firstToSecond.IsReadOnly || _secondToFirst.IsReadOnly;

        public TSecond this[TFirst key]
        {
            get => _firstToSecond[key];
            set
            {
                if (_firstToSecond.ContainsKey(key))
                {
                    _secondToFirst.Remove(_firstToSecond[key]);
                }

                _firstToSecond[key] = value;
                _secondToFirst[value] = key;
            }
        }

        public ICollection<TFirst> Keys => _firstToSecond.Keys;

        public ICollection<TSecond> Values => _firstToSecond.Values;

        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator() => _firstToSecond.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TFirst key, TSecond value)
        {
            _firstToSecond.Add(key, value);
            _secondToFirst.Add(value, key);
        }

        void ICollection<KeyValuePair<TFirst, TSecond>>.Add(KeyValuePair<TFirst, TSecond> item)
        {
            _firstToSecond.Add(item);
            _secondToFirst.Add(ReverseItem(item));
        }

        public bool ContainsKey(TFirst key)
        {
            return _firstToSecond.ContainsKey(key);
        }

        bool ICollection<KeyValuePair<TFirst, TSecond>>.Contains(KeyValuePair<TFirst, TSecond> item)
        {
            return _firstToSecond.Contains(item);
        }

        public bool TryGetValue(TFirst key, out TSecond value)
        {
            return _firstToSecond.TryGetValue(key, out value);
        }

        public bool Remove(TFirst key)
        {
            if (!_firstToSecond.TryGetValue(key, out var value))
            {
                return false;
            }

            _firstToSecond.Remove(key);
            _secondToFirst.Remove(value);

            return true;
        }

        bool ICollection<KeyValuePair<TFirst, TSecond>>.Remove(KeyValuePair<TFirst, TSecond> item)
        {
            return _firstToSecond.Remove(item);
        }

        public void Clear()
        {
            _firstToSecond.Clear();
            _secondToFirst.Clear();
        }

        void ICollection<KeyValuePair<TFirst, TSecond>>.CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex)
        {
            _firstToSecond.CopyTo(array, arrayIndex);
        }

        IEnumerable<TFirst> IReadOnlyDictionary<TFirst, TSecond>.Keys =>
            ((IReadOnlyDictionary<TFirst, TSecond>) _firstToSecond).Keys;

        IEnumerable<TSecond> IReadOnlyDictionary<TFirst, TSecond>.Values =>
            ((IReadOnlyDictionary<TFirst, TSecond>) _firstToSecond).Values;

        [SecurityPermission(
            SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, _firstToSecond);
                var array = memoryStream.ToArray();
                info.AddValue("firstToSecond", array, typeof(byte[]));
            }
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            _secondToFirst.Clear();
            foreach (var item in _firstToSecond)
            {
                _secondToFirst.Add(item.Value, item.Key);
            }
        }

        private static KeyValuePair<TItem, TKey> ReverseItem<TKey, TItem>(KeyValuePair<TKey, TItem> item)
        {
            return new KeyValuePair<TItem, TKey>(item.Value, item.Key);
        }

        private class ReverseDictionary :
            IDictionary<TSecond, TFirst>,
            IReadOnlyDictionary<TSecond, TFirst>,
            IDictionary
        {
            private readonly BiDictionary<TFirst, TSecond> _owner;

            public ReverseDictionary(BiDictionary<TFirst, TSecond> owner)
            {
                _owner = owner;
            }

            object ICollection.SyncRoot => ((ICollection) _owner._secondToFirst).SyncRoot;

            bool ICollection.IsSynchronized => ((ICollection) _owner._secondToFirst).IsSynchronized;

            bool IDictionary.IsFixedSize => ((IDictionary) _owner._secondToFirst).IsFixedSize;

            object IDictionary.this[object key]
            {
                get => ((IDictionary) _owner._secondToFirst)[key];
                set
                {
                    ((IDictionary) _owner._secondToFirst)[key] = value;
                    ((IDictionary) _owner._firstToSecond)[value] = key;
                }
            }

            ICollection IDictionary.Keys => ((IDictionary) _owner._secondToFirst).Keys;

            ICollection IDictionary.Values => ((IDictionary) _owner._secondToFirst).Values;

            IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary) _owner._secondToFirst).GetEnumerator();

            void IDictionary.Add(object key, object value)
            {
                ((IDictionary) _owner._secondToFirst).Add(key, value);
                ((IDictionary) _owner._firstToSecond).Add(value, key);
            }

            void IDictionary.Remove(object key)
            {
                var firstToSecond = (IDictionary) _owner._secondToFirst;
                if (!firstToSecond.Contains(key))
                {
                    return;
                }

                var value = firstToSecond[key];
                firstToSecond.Remove(key);
                ((IDictionary) _owner._firstToSecond).Remove(value);
            }

            bool IDictionary.Contains(object key)
            {
                return ((IDictionary) _owner._secondToFirst).Contains(key);
            }

            void ICollection.CopyTo(Array array, int index)
            {
                ((IDictionary) _owner._secondToFirst).CopyTo(array, index);
            }

            public int Count => _owner._secondToFirst.Count;

            public bool IsReadOnly => _owner._secondToFirst.IsReadOnly || _owner._firstToSecond.IsReadOnly;

            public TFirst this[TSecond key]
            {
                get => _owner._secondToFirst[key];
                set
                {
                    if (_owner._secondToFirst.ContainsKey(key))
                    {
                        _owner._firstToSecond.Remove(_owner._secondToFirst[key]);
                    }

                    _owner._secondToFirst[key] = value;
                    _owner._firstToSecond[value] = key;
                }
            }

            public ICollection<TSecond> Keys => _owner._secondToFirst.Keys;

            public ICollection<TFirst> Values => _owner._secondToFirst.Values;

            public IEnumerator<KeyValuePair<TSecond, TFirst>> GetEnumerator() => _owner._secondToFirst.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Add(TSecond key, TFirst value)
            {
                _owner._secondToFirst.Add(key, value);
                _owner._firstToSecond.Add(value, key);
            }

            void ICollection<KeyValuePair<TSecond, TFirst>>.Add(KeyValuePair<TSecond, TFirst> item)
            {
                _owner._secondToFirst.Add(item);
                _owner._firstToSecond.Add(ReverseItem(item));
            }

            public bool ContainsKey(TSecond key)
            {
                return _owner._secondToFirst.ContainsKey(key);
            }

            bool ICollection<KeyValuePair<TSecond, TFirst>>.Contains(KeyValuePair<TSecond, TFirst> item)
            {
                return _owner._secondToFirst.Contains(item);
            }

            public bool TryGetValue(TSecond key, out TFirst value)
            {
                return _owner._secondToFirst.TryGetValue(key, out value);
            }

            public bool Remove(TSecond key)
            {
                if (!_owner._secondToFirst.TryGetValue(key, out var value))
                {
                    return false;
                }

                _owner._secondToFirst.Remove(key);
                _owner._firstToSecond.Remove(value);

                return true;
            }

            bool ICollection<KeyValuePair<TSecond, TFirst>>.Remove(KeyValuePair<TSecond, TFirst> item)
            {
                return _owner._secondToFirst.Remove(item);
            }

            public void Clear()
            {
                _owner._secondToFirst.Clear();
                _owner._firstToSecond.Clear();
            }

            void ICollection<KeyValuePair<TSecond, TFirst>>.CopyTo(
                KeyValuePair<TSecond, TFirst>[] array,
                int arrayIndex)
            {
                _owner._secondToFirst.CopyTo(array, arrayIndex);
            }

            IEnumerable<TSecond> IReadOnlyDictionary<TSecond, TFirst>.Keys =>
                ((IReadOnlyDictionary<TSecond, TFirst>) _owner._secondToFirst).Keys;

            IEnumerable<TFirst> IReadOnlyDictionary<TSecond, TFirst>.Values =>
                ((IReadOnlyDictionary<TSecond, TFirst>) _owner._secondToFirst).Values;
        }
    }
}
