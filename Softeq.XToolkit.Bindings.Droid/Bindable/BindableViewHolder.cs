// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    public abstract class BindableViewHolder<TViewModel>
        : RecyclerView.ViewHolder, IBindableViewHolder
    {
        private IDisposable _itemViewClickSubscription;

        protected BindableViewHolder(View itemView) : base(itemView)
        {
        }

        public event EventHandler ItemClicked;

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; }

        protected TViewModel ViewModel => (TViewModel) DataContext;

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }

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
