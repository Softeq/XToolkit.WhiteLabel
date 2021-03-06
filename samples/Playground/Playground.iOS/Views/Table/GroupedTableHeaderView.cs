// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using Foundation;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.Common.iOS.Extensions;
using UIKit;

namespace Playground.iOS.Views.Table
{
    public partial class GroupedTableHeaderView : BindableTableViewHeaderFooterView<ProductHeaderViewModel>
    {
        #region init

        public static readonly NSString Key = new NSString(nameof(GroupedTableHeaderView));
        public static readonly UINib Nib;

        static GroupedTableHeaderView() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected GroupedTableHeaderView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        ~GroupedTableHeaderView()
        {
            Console.WriteLine($"Finalized: {nameof(GroupedTableHeaderView)}");
        }

        #endregion

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

        partial void InfoButtonAction(NSObject _)
        {
            ViewModel.InfoCommand?.Execute(ViewModel);
        }

        partial void GenerateButtonAction(NSObject _)
        {
            ViewModel.GenerateCommand?.Execute(ViewModel);
        }

        partial void AddButtonAction(NSObject _)
        {
            ViewModel.AddCommand?.Execute(ViewModel);
        }
    }
}
