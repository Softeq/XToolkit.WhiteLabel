// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Softeq.XToolkit.Common.Collections
{
    public class BiDictionary<TLeft, TRight> : IEnumerable
    {
        private readonly Dictionary<TLeft, TRight> _lefts = new Dictionary<TLeft, TRight>();
        private readonly Dictionary<TRight, TLeft> _rights = new Dictionary<TRight, TLeft>();

        public int Count => _lefts.Count;

        public TRight this[TLeft i]
        {
            get => _lefts[i];
            set
            {
                if (_lefts.ContainsKey(i))
                {
                    RemoveLeft(i);
                }

                Add(i, value);
            }
        }

        public TLeft this[TRight i]
        {
            get => _rights[i];
            set
            {
                if (_rights.ContainsKey(i))
                {
                    RemoveRight(i);
                }

                Add(value, i);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _lefts.GetEnumerator();
        }

        public void Add(KeyValuePair<TLeft, TRight> pair)
        {
            Add(pair.Key, pair.Value);
        }

        public void Add(TLeft left, TRight right)
        {
            Debug.Assert(_lefts.ContainsKey(left) == false, "No Key", "There is already left '{0}'", left);
            Debug.Assert(_rights.ContainsKey(right) == false, "No Key", "There is already right '{0}'", right);

            _lefts.Add(left, right);
            _rights.Add(right, left);
        }

        public void RemoveLeft(TLeft left)
        {
            Debug.Assert(_lefts.ContainsKey(left), "No Key", "There is no left '{0}'", left);
            _rights.Remove(_lefts[left]);
            _lefts.Remove(left);
        }

        public void RemoveRight(TRight right)
        {
            Debug.Assert(_rights.ContainsKey(right), "No Key", "There is no right '{0}'", right);
            _lefts.Remove(_rights[right]);
            _rights.Remove(right);
        }

        public bool ContainsLeft(TLeft left)
        {
            return _lefts.ContainsKey(left);
        }

        public bool ContainsRight(TRight right)
        {
            return _rights.ContainsKey(right);
        }

        public bool TryGetLeft(TRight right, out TLeft outLeft)
        {
            return _rights.TryGetValue(right, out outLeft);
        }

        public bool TryGetRight(TLeft left, out TRight outRight)
        {
            return _lefts.TryGetValue(left, out outRight);
        }

        public TLeft GetLeft(TRight right)
        {
            return _rights[right];
        }

        public TRight GetRight(TLeft left)
        {
            return _lefts[left];
        }

        public void SetLeft(TRight right, TLeft left)
        {
            this[right] = left;
        }

        public void SetRight(TLeft left, TRight right)
        {
            this[left] = right;
        }

        public IEnumerable<TRight> GetRights()
        {
            return _rights.Keys;
        }

        public IEnumerable<TLeft> GetLefts()
        {
            return _lefts.Keys;
        }

        public void Clear()
        {
            _lefts.Clear();
            _rights.Clear();
        }
    }
}