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

        public event EventHandler ItemClicked;

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext => BindingContext;

        protected TViewModel BindingContext { get; private set; }

        protected BindableViewHolder(View itemView) : base(itemView)
        {
        }

        internal void SetBindingContext(TViewModel viewModel)
        {
            BindingContext = viewModel;

            Bindings.DetachAllAndClear();

            SetBindings();
        }

        protected abstract void SetBindings();

        public virtual void OnAttachedToWindow()
        {
            if (_itemViewClickSubscription == null)
            {
                // TODO YP: add event to linker ignore 
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
            BindingContext = default(TViewModel);

            Bindings.DetachAllAndClear();
        }

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            ItemClicked?.Invoke(this, e);
        }
    }
}
