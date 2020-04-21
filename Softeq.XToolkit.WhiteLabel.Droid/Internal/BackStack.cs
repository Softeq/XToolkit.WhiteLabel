// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.WhiteLabel.Droid.Internal
{
    internal class BackStack<T>
    {
        private readonly Stack<T> _backStack;

        internal BackStack()
        {
            _backStack = new Stack<T>();
        }

        internal bool IsEmpty => _backStack.Count == 0;

        internal bool CanGoBack => _backStack.Count > 1;

        internal void Add(T entry) => _backStack.Push(entry);

        internal T CurrentWithRemove() => _backStack.Pop();

        internal T Current() => _backStack.Peek();

        internal void Clear() => _backStack.Clear();

        internal IReadOnlyList<TResult> Dump<TResult>(Func<T, TResult> selector)
        {
            return _backStack.Select(selector).ToArray();
        }

        internal T ResetToFirst()
        {
            while (_backStack.Count > 1)
            {
                _backStack.Pop();
            }

            return _backStack.Pop();
        }

        internal void GoBackWhile(Func<T, bool> canGoBack)
        {
            while (canGoBack(_backStack.Peek()))
            {
                _backStack.Pop();
            }
        }
    }
}
