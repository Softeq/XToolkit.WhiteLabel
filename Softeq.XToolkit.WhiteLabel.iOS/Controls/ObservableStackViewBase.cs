// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public abstract class ObservableStackViewBase<TItem, TView> : UIStackView where TView : UIView
    {
        private Action<TItem, TView>? _bindAction;
        private Action<TItem, TView>? _refreshBindingAction;

        protected ObservableRangeCollection<TItem>? Collection;

        protected ObservableStackViewBase(IntPtr handle) : base(handle)
        {
        }

        public void SetData(
            ObservableRangeCollection<TItem> items,
            Action<TItem, TView>? bindAction = null,
            Action<TItem, TView>? refreshBindingAction = null)
        {
            Collection = items;
            _bindAction = bindAction;
            _refreshBindingAction = refreshBindingAction;
            DoAttachBindings();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            InitView();
        }

        public virtual void DoAttachBindings()
        {
            if (Collection == null)
            {
                return;
            }

            Collection.CollectionChanged += OnCollectionChanged;

            for (var i = 0; i < Collection.Count; i++)
            {
                var item = Collection[i];
                TView view;

                if (ArrangedSubviews.Length > i)
                {
                    view = (TView) ArrangedSubviews[i];
                    _refreshBindingAction?.Invoke(item, view);
                }
                else
                {
                    view = AddView(item);
                    _bindAction?.Invoke(item, view);
                }
            }
        }

        public void DoDetachBindings()
        {
            if (Collection != null)
            {
                Collection.CollectionChanged -= OnCollectionChanged;
            }

            _bindAction = null;
            _refreshBindingAction = null;
        }

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ActionAdd(e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ActionRemove(e);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    ActionAdd(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Collection));
                    break;
            }
        }

        protected virtual void LayoutSubView(TView view)
        {
        }

        protected abstract TView GetView(TItem item);

        private TView GetViewByIndex(int index)
        {
            if (index < ArrangedSubviews.Length)
            {
                return (TView) ArrangedSubviews[index];
            }

            return default!;
        }

        private void ActionAdd(NotifyCollectionChangedEventArgs e)
        {
            foreach (TItem item in e.NewItems)
            {
                var view = AddView(item);
                _bindAction?.Invoke(item, view);
            }
        }

        private TView AddView(TItem item)
        {
            var view = GetView(item);
            AddArrangedSubview(view);
            LayoutSubView(view);
            return view;
        }

        private void ActionRemove(NotifyCollectionChangedEventArgs e)
        {
            var startIndex = e.OldStartingIndex;
            for (var i = 0; i < e.OldItems.Count; i++)
            {
                var index = startIndex + i;
                var view = GetViewByIndex(index);
                RemoveArrangedSubview(view);
                view.RemoveFromSuperview();
            }
        }

        private void Clear()
        {
            while (ArrangedSubviews.Length != 0)
            {
                var view = GetViewByIndex(0);
                RemoveArrangedSubview(view);
                view.RemoveFromSuperview();
            }
        }

        private void InitView()
        {
            Axis = UILayoutConstraintAxis.Vertical;
            Alignment = UIStackViewAlignment.Center;
            Distribution = UIStackViewDistribution.Fill;
            TranslatesAutoresizingMaskIntoConstraints = false;
        }
    }
}
