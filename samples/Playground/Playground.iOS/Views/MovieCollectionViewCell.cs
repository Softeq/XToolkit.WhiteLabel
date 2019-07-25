// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using Foundation;
using UIKit;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.Bindings.Extensions;
using Playground.Models;

namespace Playground.iOS.Views
{
    public partial class MovieCollectionViewCell : BindableCollectionViewCell<ItemViewModel>
    {
        public static readonly NSString Key = new NSString(nameof(MovieCollectionViewCell));
        public static readonly UINib Nib;

        static MovieCollectionViewCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected MovieCollectionViewCell(IntPtr handle) : base(handle)
        {
        }

        public override void SetBindings()
        {
            Poster.Image = null;

            ImageService.Instance.LoadUrl(ViewModel.IconUrl).Into(Poster);

            this.Bind(() => ViewModel.Title, () => Title.Text);
        }
    }
}
