// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using Foundation;
using ObjCRuntime;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.Views.Collections
{
    public partial class ProductViewCell : BindableCollectionViewCell<ProductViewModel>
    {
        public static readonly NSString Key = new NSString(nameof(ProductViewCell));
        public static readonly UINib Nib;

        static ProductViewCell() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected ProductViewCell(NativeHandle handle)
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        ~ProductViewCell()
        {
            Console.WriteLine($"Finalized: {nameof(ProductViewCell)}");
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
