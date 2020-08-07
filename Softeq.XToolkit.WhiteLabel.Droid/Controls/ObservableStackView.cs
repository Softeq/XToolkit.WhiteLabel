// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections;
using System.Collections.Specialized;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    public abstract class ObservableStackView<TModel, TView> : LinearLayout where TView : View
    {
        private Action<TModel, TView>? _bindAction;
        private Action<TModel, TView>? _refreshBindingAction;

        protected ObservableRangeCollection<TModel>? Collection;

        protected ObservableStackView(Context context)
            : base(context)
        {
        }

        protected ObservableStackView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        protected ObservableStackView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        protected ObservableStackView(IntPtr handle, JniHandleOwnership owner)
            : base(handle, owner)
        {
        }

        public abstract TView GetView(TModel item);

        public TView? GetViewByIndex(int index)
        {
            if (index < ChildCount)
            {
                return (TView) GetChildAt(index);
            }

            return null;
        }

        public void SetData(
            ObservableRangeCollection<TModel> items,
            Action<TModel, TView>? bindAction = null,
            Action<TModel, TView>? refreshBindingAction = null)
        {
            Collection = items;
            _bindAction = bindAction;
            _refreshBindingAction = refreshBindingAction;
            DoAttachBindings();
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

                if (ChildCount > i)
                {
                    view = (TView) GetChildAt(i);
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
                    break;
            }
        }

        protected void ActionAdd(NotifyCollectionChangedEventArgs e)
        {
            AddItems(e.NewItems);
        }

        protected void AddItems(IList models)
        {
            foreach (TModel item in models)
            {
                var view = AddView(item);
                _bindAction?.Invoke(item, view);
            }
        }

        private TView AddView(TModel item)
        {
            var view = GetView(item);
            AddView(view);
            return view;
        }

        private void Clear()
        {
            while (ChildCount > 0)
            {
                RemoveViewAt(0);
            }
        }

        private void ActionRemove(NotifyCollectionChangedEventArgs e)
        {
            var startIndex = e.OldStartingIndex;
            for (var i = 0; i < e.OldItems.Count; i++)
            {
                RemoveViewAt(i + startIndex);
            }
        }
    }
}
