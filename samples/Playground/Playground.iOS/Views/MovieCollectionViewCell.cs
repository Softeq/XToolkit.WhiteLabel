// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using Foundation;
using Playground.Models;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.Views
{
    public partial class MovieCollectionViewCell : BindableCollectionViewCell<ItemViewModel>
    {
        public static readonly NSString Key = new NSString(nameof(MovieCollectionViewCell));
        public static readonly UINib Nib;

        static MovieCollectionViewCell() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected MovieCollectionViewCell(IntPtr handle) : base(handle)
        {
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            ImageService.Instance.LoadUrl(ViewModel.IconUrl).Into(Poster);

            this.Bind(() => ViewModel.Title, () => Title.Text);
        }

        public override void DoDetachBindings()
        {
            base.DoDetachBindings();

            Poster.Image = null;
        }
    }
}
