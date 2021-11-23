// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using Foundation;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.Views.Table
{
    public partial class ProductTableViewCell : BindableTableViewCell<ProductViewModel>
    {
        public static readonly NSString Key = new NSString(nameof(ProductTableViewCell));
        public static readonly UINib Nib;

        static ProductTableViewCell()
        {
            Nib = UINib.FromName(nameof(ProductTableViewCell), NSBundle.MainBundle);
        }

        protected ProductTableViewCell(IntPtr handle)
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        // TODO YP: Doesn't support correct use BindingMode.TwoWay (cells not disposed)
        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            ImageService.Instance.LoadUrl(ViewModel.PhotoUrl).Into(PhotoImage);

            this.Bind(() => ViewModel.Title, () => NameLabel.Text);
            this.Bind(() => ViewModel.Count, () => CountField.Text);
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            PhotoImage.Image = null;
            CountField.Text = string.Empty;
        }

        partial void AddToCartAction(NSObject sender)
        {
            // main way for handle custom click by item

            ViewModel.AddToBasketCommand?.Execute(ViewModel);
        }
    }
}
