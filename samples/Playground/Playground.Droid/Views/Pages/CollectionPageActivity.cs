// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.Droid.Bindable;
using Softeq.XToolkit.Bindings.Droid.Bindable.Collection;

using FFImageLoading;
using FFImageLoading.Views;

using Playground.Models;
using Playground.ViewModels.Pages;

namespace Playground.Droid.Views.Pages
{
    [Activity(Theme = "@style/AppTheme")]
    public class CollectionPageActivity : ActivityBase<CollectionPageViewModel>
    {
        private RecyclerView _recyclerView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_collection);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.activity_collection_recycler_view);

            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            _recyclerView.HasFixedSize = true;

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            var adapter = new BindableRecyclerViewAdapter<ItemViewModel, MovieCollectionViewHolder>(
                ViewModel.ItemModels,
                Resource.Layout.item_movie);

            adapter.ItemClick = ViewModel.SelectItemCommand;

            _recyclerView.SetAdapter(adapter);
        }
    }

    public class MovieCollectionViewHolder : BindableViewHolder<ItemViewModel>
    {
        private readonly ImageViewAsync _image;
        private readonly TextView _name;

        public MovieCollectionViewHolder(View view) : base(view)
        {
            _image = view.FindViewById<ImageViewAsync>(Resource.Id.item_movie_image);
            _name = view.FindViewById<TextView>(Resource.Id.item_movie_name);
        }

        protected override void SetBindings()
        {
            _image.SetImageDrawable(null);

            ImageService.Instance
                .LoadUrl(BindingContext.IconUrl)
                .Into(_image);

            this.Bind(() => BindingContext.Title, () => _name.Text);
        }
    }
}
