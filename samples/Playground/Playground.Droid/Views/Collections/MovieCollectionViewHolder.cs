// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;
using Android.Widget;
using FFImageLoading;
using Playground.Models;
using Softeq.XToolkit.Bindings.Droid.Bindable;
using Softeq.XToolkit.Bindings.Extensions;

namespace Playground.Droid.Views.Collections
{
    [BindableViewHolderLayout(Resource.Layout.item_movie)]
    public class MovieCollectionViewHolder : BindableViewHolder<ItemViewModel>
    {
        private readonly ImageView _image;
        private readonly TextView _name;

        public MovieCollectionViewHolder(View view) : base(view)
        {
            _image = view.FindViewById<ImageView>(Resource.Id.item_movie_image)!;
            _name = view.FindViewById<TextView>(Resource.Id.item_movie_name)!;
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            ImageService.Instance
                .LoadUrl(ViewModel.IconUrl)
                .Into(_image);

            this.Bind(() => ViewModel.Title, () => _name.Text);
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            _image.SetImageDrawable(null);
        }
    }
}
