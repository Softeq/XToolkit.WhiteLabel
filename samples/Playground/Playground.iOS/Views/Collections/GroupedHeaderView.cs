// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using Foundation;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.Common.iOS.Extensions;
using UIKit;

namespace Playground.iOS.Views.Collections
{
    public partial class GroupedHeaderView : BindableUICollectionReusableView<ProductHeaderViewModel>
    {
        #region init

        public static readonly NSString Key = new NSString(nameof(GroupedHeaderView));
        public static readonly UINib Nib;

        static GroupedHeaderView() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected GroupedHeaderView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        #endregion

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ContainerView
                .WithBorder(1, UIColor.Gray.CGColor)
                .WithCornerRadius(10)
                .WithShadow(CGSize.Empty, UIColor.Gray, 0.2, 10);
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            TitleLabel.Text = $"{ViewModel.Category}th";
        }

        partial void InfoButtonAction(NSObject sender)
        {
            if (ViewModel.InfoCommand.CanExecute(ViewModel))
            {
                ViewModel.InfoCommand.Execute(ViewModel);
            }
        }
    }
}
