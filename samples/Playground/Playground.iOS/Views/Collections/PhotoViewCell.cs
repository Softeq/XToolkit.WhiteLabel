// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using Foundation;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.Views.Collections
{
    public partial class PhotoViewCell : BindableCollectionViewCell<ProductViewModel>
    {
        #region init

        public static readonly NSString Key = new NSString(nameof(PhotoViewCell));
        public static readonly UINib Nib;

        static PhotoViewCell() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected PhotoViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        #endregion

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            ImageService.Instance.LoadUrl(ViewModel.PhotoUrl).Into(PhotoImage);

            this.Bind(() => ViewModel.Title, () => NameLabel.Text);
            this.Bind(() => ViewModel.Count, () => CountField.Text, BindingMode.TwoWay);
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            PhotoImage.Image = null;
            CountField.Text = string.Empty;
        }

        partial void AddToCartAction(NSObject sender)
        {
            var command = ViewModel.AddToCartCommand;

            if (command.CanExecute(ViewModel))
            {
                command.Execute(ViewModel);
            }
        }
    }
}

