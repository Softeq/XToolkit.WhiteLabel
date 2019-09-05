// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Support.V7.Widget;
using Android.Views;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Command;
using Object = Java.Lang.Object;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    /// <summary>
    ///     Bindable RecyclerViewAdapter
    /// </summary>
    /// <typeparam name="TViewModel">Item ViewModel</typeparam>
    /// <typeparam name="TViewHolder">Item ViewHolder</typeparam>
    public class BindableRecyclerViewAdapter<TViewModel, TViewHolder>
        : ObservableRecyclerViewAdapter<TViewModel>
        where TViewHolder : BindableViewHolder<TViewModel>
    {
        private ICommand<TViewModel> _itemClick;

        public BindableRecyclerViewAdapter(IList<TViewModel> items)
            : base(items, null, null)
        {
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
                    throw new ArgumentException(
                        "Changing ItemClick may cause inconsistencies where some items still call the old command.",
                        nameof(ItemClick));
                }

                _itemClick = value;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var viewHolderType = typeof(TViewHolder);
            var view = GetLayoutForViewHolder(parent, viewHolderType);
            return (RecyclerView.ViewHolder) Activator.CreateInstance(viewHolderType, view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var bindableViewHolder = (IBindableViewHolder) holder;

            bindableViewHolder.ReloadDataContext(DataSource[position]);

            bindableViewHolder.ItemClicked -= OnItemViewClick;
            bindableViewHolder.ItemClicked += OnItemViewClick;

            CheckIfLastItemRequested(position);
        }

        public override void OnViewAttachedToWindow(Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var bindableViewHolder = (IBindableViewHolder) holder;

            bindableViewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            var bindableViewHolder = (IBindableViewHolder) holder;

            bindableViewHolder.OnDetachedFromWindow();

            base.OnViewDetachedFromWindow(holder);
        }

        public override void OnViewRecycled(Object holder)
        {
            var bindableViewHolder = (IBindableViewHolder) holder;

            bindableViewHolder.ItemClicked -= OnItemViewClick;
            bindableViewHolder.OnViewRecycled();
        }

        /// <summary>
        ///     By default, force recycling a view if it has animations
        /// </summary>
        public override bool OnFailedToRecycleView(Object holder)
        {
            return true;
        }

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

        protected virtual View GetLayoutForViewHolder(ViewGroup parent, Type viewHolderType)
        {
            if (viewHolderType == null)
            {
                throw new ArgumentNullException(nameof(viewHolderType), "Check ViewHolder declarations.");
            }

            if (Attribute.GetCustomAttribute(viewHolderType, typeof(BindableViewHolderLayoutAttribute))
                is BindableViewHolderLayoutAttribute attr)
            {
                return LayoutInflater.From(parent.Context).Inflate(attr.LayoutId, parent, false);
            }

            return GetCustomLayoutForViewHolder(parent, viewHolderType);
        }

        protected virtual View GetCustomLayoutForViewHolder(ViewGroup parent, Type viewHolderType)
        {
            throw new NotImplementedException(
                "Tried to use custom inflating of ViewHolder layout, please implement this method. " +
                $"Or use {nameof(BindableViewHolderLayoutAttribute)} for auto-inflating ViewHolder layout.");
        }
    }
}
