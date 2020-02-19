// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Views;
using Android.Widget;
using FFImageLoading;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.Droid.Bindable;

namespace Playground.Droid.Views.Collections
{
    [BindableViewHolderLayout(Resource.Layout.item_product)]
    public class ProductViewHolder : BindableViewHolder<ProductViewModel>
    {
        private readonly ImageView _image;
        private readonly TextView _title;
        private readonly ImageButton _addToCartButton;
        private readonly EditText _count;

        public ProductViewHolder(View view) : base(view)
        {
            _image = view.FindViewById<ImageView>(Resource.Id.item_product_photo_img);
            _title = view.FindViewById<TextView>(Resource.Id.item_product_title_lbl);
            _addToCartButton = view.FindViewById<ImageButton>(Resource.Id.item_product_add_cart_btn);
            _count = view.FindViewById<EditText>(Resource.Id.item_product_count_txt);
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            ImageService.Instance
                .LoadUrl(ViewModel.PhotoUrl)
                .Into(_image);

            this.Bind(() => ViewModel.Title, () => _title.Text);
            this.Bind(() => ViewModel.Count, () => _count.Text, BindingMode.TwoWay);

            _addToCartButton.Click += AddToCartButton_Click;
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            _image.SetImageDrawable(null);
            _count.Text = "";

            _addToCartButton.Click -= AddToCartButton_Click;
        }

        private void AddToCartButton_Click(object sender, System.EventArgs e)
        {
            ViewModel.AddToBasketCommand!.Execute(ViewModel);
        }
    }

    [BindableViewHolderLayout(Resource.Layout.item_product_header)]
    public class ProductHeaderViewHolder : BindableViewHolder<ProductHeaderViewModel>
    {
        private readonly TextView _title;
        private readonly Button _infoButton;
        private readonly Button _generateButton;
        private readonly Button _addButton;

        public ProductHeaderViewHolder(View view) : base(view)
        {
            _title = view.FindViewById<TextView>(Resource.Id.item_product_header_title_lbl);
            _generateButton = view.FindViewById<Button>(Resource.Id.item_product_header_generate_btn);
            _addButton = view.FindViewById<Button>(Resource.Id.item_product_header_add_btn);
            _infoButton = view.FindViewById<Button>(Resource.Id.item_product_header_info_btn);
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            _title.Text = $"{ViewModel.Category}th";

            _infoButton.Click += InfoButton_Click;
            _generateButton.Click += _generateButton_Click;
            _addButton.Click += _addButton_Click;
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            _infoButton.Click -= InfoButton_Click;
            _generateButton.Click -= _generateButton_Click;
            _addButton.Click -= _addButton_Click;
        }

        private void _addButton_Click(object sender, System.EventArgs e)
        {
            ViewModel.AddCommand!.Execute(ViewModel);
        }

        private void _generateButton_Click(object sender, System.EventArgs e)
        {
            ViewModel.GenerateCommand!.Execute(ViewModel);
        }

        private void InfoButton_Click(object sender, System.EventArgs e)
        {
            ViewModel.InfoCommand!.Execute(ViewModel);
        }
    }

    public class ProductFooterViewHolder : BindableViewHolder<ProductHeaderViewModel>
    {
        private readonly TextView _textView;

        public ProductFooterViewHolder(View view) : base(view)
        {
            _textView = (TextView) view;
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            _textView.Text = $"{ViewModel.Category}th footer.";
        }
    }
}
