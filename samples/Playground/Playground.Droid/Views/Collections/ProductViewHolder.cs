// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;
using Android.Widget;
using FFImageLoading;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.Droid.Bindable;

namespace Playground.Droid.Views.Collections
{
    public class ProductViewHolder : BindableViewHolder<ProductViewModel>
    {
        private readonly ImageView _image;
        private readonly TextView _title;

        public ProductViewHolder(View view) : base(view)
        {
            _image = view.FindViewById<ImageView>(Resource.Id.item_movie_image);
            _title = view.FindViewById<TextView>(Resource.Id.item_movie_name);
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            ImageService.Instance
                .LoadUrl(ViewModel.PhotoUrl)
                .Into(_image);

            this.Bind(() => ViewModel.Title, () => _title.Text);
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            _image.SetImageDrawable(null);
        }
    }
}
