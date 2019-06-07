// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using System.Diagnostics;

using Softeq.XToolkit.Bindings.Droid;
using Softeq.XToolkit.WhiteLabel.Droid;

using FFImageLoading;
using FFImageLoading.Views;

using Playground.Models;
using Playground.ViewModels.Pages;
using System.ComponentModel;
using System;
using Softeq.XToolkit.Common.WeakSubscription;

namespace Playground.Droid.Views.Pages
{
    [Activity(Theme = "@style/AppTheme")]
    public class CollectionPageActivity : ActivityBase<CollectionPageViewModel>
    {
        private RecyclerView _recyclerView;
        private LinearLayoutManager _layoutManager;

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_collection);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_collection_recycler_view);

            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            _recyclerView.HasFixedSize = true;

            // use a linear layout manager
            _layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(_layoutManager);

            var adapter = new ObservableRecyclerViewAdapter<ItemModel>(
                ViewModel.Items,
                (parent, viewType) =>
                {
                    var cell = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_movie, parent, false);
                    return new MovieCollectionViewHolder(cell);
                },
                (viewHolder, index, viewModel) =>
                {
                    ((MovieCollectionViewHolder) viewHolder).BindViewModel(viewModel);
                });

            adapter.ItemClick = ViewModel.SelectItemCommand;

            _recyclerView.SetAdapter(adapter);
        }
    }

    public class MovieCollectionViewHolder : BindableViewHolder<ItemModel>
    {
        private readonly ImageViewAsync _image;
        private readonly TextView _name;

        public MovieCollectionViewHolder(View view) : base(view)
        {
            _image = view.FindViewById<ImageViewAsync>(Resource.Id.item_movie_image);
            _name = view.FindViewById<TextView>(Resource.Id.item_movie_name);
        }

        public override void BindViewModel(ItemModel viewModel)
        {
            ImageService.Instance
                .LoadUrl(viewModel.IconUrl)
                .IntoAsync(_image);

            _name.Text = viewModel.Title;
        }
    }

    public abstract class BindableViewHolder<TViewModel> : RecyclerView.ViewHolder, IBindableViewHolder
    {
        private IDisposable _itemViewClickSubscription;

        public event EventHandler Click;

        public BindableViewHolder(View itemView) : base(itemView)
        {
        }

        public abstract void BindViewModel(TViewModel viewModel);

        public virtual void OnAttachedToWindow()
        {
            if (_itemViewClickSubscription == null)
            {
                _itemViewClickSubscription = new WeakEventSubscription<View>(ItemView, nameof(ItemView.Click), OnItemViewClick);
            }

            Debug.WriteLine("Attached");
        }

        public void OnDetachedFromWindow()
        {
            Clean();

            Debug.WriteLine("Detached");
        }

        public void OnViewRecycled()
        {
            Clean();
            Debug.WriteLine("Recycled");
        }

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            Click?.Invoke(this, e);
        }

        private void Clean()
        {
            _itemViewClickSubscription?.Dispose();
        }
    }

}
