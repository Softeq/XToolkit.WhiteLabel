// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using Foundation;
using Playground.Models;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.Views.Collections
{
    public partial class PhotoViewCell : BindableCollectionViewCell<ItemViewModel>
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

            ImageService.Instance.LoadUrl(ViewModel.IconUrl).Into(PhotoImage);

            this.Bind(() => ViewModel.Title, () => NameLabel.Text);
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            PhotoImage.Image = null;
        }
    }
}

