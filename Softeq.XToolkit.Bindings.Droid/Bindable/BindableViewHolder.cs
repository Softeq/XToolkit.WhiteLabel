// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.WeakSubscription;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    public abstract class BindableViewHolder<TViewModel>
        : RecyclerView.ViewHolder, IBindableViewHolder
    {
        private IDisposable _itemViewClickSubscription;

        protected BindableViewHolder(View itemView) : base(itemView)
        {
            Bindings = new List<Binding>();
        }

        protected TViewModel ViewModel => (TViewModel) DataContext;

        public event EventHandler ItemClicked;

        public object DataContext { get; set; }
        public List<Binding> Bindings { get; }

        public virtual void OnAttachedToWindow()
        {
            if (_itemViewClickSubscription == null)
            {
                _itemViewClickSubscription = new WeakEventSubscription<View>(ItemView, nameof(ItemView.Click), OnItemViewClick);
            }
        }

        public virtual void OnDetachedFromWindow()
        {
            _itemViewClickSubscription?.Dispose();
            _itemViewClickSubscription = null;
        }

        public virtual void OnViewRecycled()
        {
            DataContext = default(TViewModel);

            DoDetachBindings();
        }

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            ItemClicked?.Invoke(this, e);
        }

        public virtual void DoAttachBindings()
        {
        }

        public virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }
    }
}
