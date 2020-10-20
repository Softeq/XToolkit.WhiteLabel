// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    /// <summary>
    ///     An implementation of <see cref="IBackStackManager"/> interface
    ///     that contains methods for managing ViewModels back stack.
    /// </summary>
    public class BackStackManager : IBackStackManager
    {
        private readonly Stack<IViewModelBase> _backStack = new Stack<IViewModelBase>();

        /// <inheritdoc/>
        public int Count => _backStack.Count;

        /// <inheritdoc/>
        public void PushViewModel(IViewModelBase viewModel)
        {
            _backStack.Push(viewModel);
        }

        /// <inheritdoc/>
        public IViewModelBase PopViewModel()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Back stack is empty");
            }

            return _backStack.Pop();
        }

        /// <inheritdoc/>
        public bool TryPopViewModel(out IViewModelBase result)
        {
            return _backStack.TryPop(out result);
        }

        /// <inheritdoc/>
        public IViewModelBase PeekViewModel()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Back stack is empty");
            }

            return _backStack.Peek();
        }

        /// <inheritdoc/>
        public bool TryPeekViewModel(out IViewModelBase result)
        {
            return _backStack.TryPeek(out result);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _backStack.Clear();
        }
    }
}
