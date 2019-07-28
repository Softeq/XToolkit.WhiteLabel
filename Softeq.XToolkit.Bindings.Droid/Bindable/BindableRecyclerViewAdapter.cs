// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Command;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    /// <summary>
    /// Bindable RecyclerViewAdapter
    /// </summary>
    /// <typeparam name="TViewModel">Item ViewModel</typeparam>
    /// <typeparam name="TViewHolder">Item ViewHolder</typeparam>
    public class BindableRecyclerViewAdapter<TViewModel, TViewHolder>
        : ObservableRecyclerViewAdapter<TViewModel>
        where TViewHolder : BindableViewHolder<TViewModel>
    {
        private readonly int _itemLayoutId;

        private ICommand<TViewModel> _itemClick;

        public BindableRecyclerViewAdapter(
            IList<TViewModel> items,
            int itemLayoutId)
            : base(items, null, SetDataContext)
        {
            _itemLayoutId = itemLayoutId;
        }

        public ICommand<TViewModel> ItemClick
        {
            get => _itemClick;
            set
            {
                if (ReferenceEquals(_itemClick, value))
                {
                    return;
                }

                if (_itemClick != null && value != null)
                {
                    Log.Warn(nameof(BindableRecyclerViewAdapter<TViewModel, TViewHolder>),
                        "Changing ItemClick may cause inconsistencies where some items still call the old command.");
                }

                _itemClick = value;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var cell = LayoutInflater.From(parent.Context).Inflate(_itemLayoutId, parent, false);
            return (RecyclerView.ViewHolder) Activator.CreateInstance(typeof(TViewHolder), cell);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);

            var bindableViewHolder = (IBindableViewHolder) holder;

            bindableViewHolder.ItemClicked -= OnItemViewClick;
            bindableViewHolder.ItemClicked += OnItemViewClick;
        }

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var bindableViewHolder = (IBindableViewHolder) holder;

            bindableViewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            var bindableViewHolder = (IBindableViewHolder) holder;

            bindableViewHolder.OnDetachedFromWindow();

            base.OnViewDetachedFromWindow(holder);
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            var bindableViewHolder = (IBindableViewHolder) holder;

            bindableViewHolder.ItemClicked -= OnItemViewClick;
            bindableViewHolder.OnViewRecycled();
        }

        /// <summary>
        /// By default, force recycling a view if it has animations
        /// </summary>
        public override bool OnFailedToRecycleView(Java.Lang.Object holder) => true;

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            var bindableViewHolder = (IBindableViewHolder) sender;

            ExecuteCommandOnItem(ItemClick, bindableViewHolder.DataContext);
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, object itemDataContext)
        {
            if (command != null && itemDataContext != null && command.CanExecute(itemDataContext))
            {
                command.Execute(itemDataContext);
            }
        }

        private static void SetDataContext(RecyclerView.ViewHolder viewHolder, int viewType, TViewModel viewModel)
        {
            var bindable = (IBindable) viewHolder;

            bindable.SetDataContext(viewModel);
        }
    }
}
