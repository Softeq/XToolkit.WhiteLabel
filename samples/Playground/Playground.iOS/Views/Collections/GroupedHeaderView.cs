// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.Common.iOS.Extensions;
using UIKit;

namespace Playground.iOS.Views.Collections
{
    public partial class GroupedHeaderView : BindableUICollectionReusableView<ProductHeaderViewModel>
    {
        public static readonly NSString Key = new NSString(nameof(GroupedHeaderView));
        public static readonly UINib Nib;

        static GroupedHeaderView() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected GroupedHeaderView(NativeHandle handle)
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        ~GroupedHeaderView()
        {
            Console.WriteLine($"Finalized: {nameof(GroupedHeaderView)}");
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ContainerView
                .WithBorder(1, UIColor.Gray.CGColor)
                .WithCornerRadius(10)
                .WithShadow(new CGSize(0, 5), UIColor.Red, 0.3, 10);
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            TitleLabel.Text = $"{ViewModel.Category}th";
        }

        partial void InfoButtonAction(NSObject sender)
        {
            ViewModel.InfoCommand?.Execute(ViewModel);
        }

        partial void GenerateButtonAction(NSObject sender)
        {
            ViewModel.GenerateCommand?.Execute(ViewModel);
        }

        partial void AddButtonAction(NSObject sender)
        {
            ViewModel.AddCommand?.Execute(ViewModel);
        }
    }
}
