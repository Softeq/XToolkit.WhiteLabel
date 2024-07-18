// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using ObjCRuntime;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.Views.Table
{
    public class GroupedTableFooterView : BindableTableViewHeaderFooterView<ProductHeaderViewModel>
    {
        private readonly UILabel _label;

        public GroupedTableFooterView(NativeHandle handle)
            : base(handle)
        {
            _label = new UILabel(new CGRect(0, 0, 300, 20));
            _label.TextColor = UIColor.DarkGray;

            // ReSharper disable once VirtualMemberCallInConstructor
            AddSubview(_label);
        }

        ~GroupedTableFooterView()
        {
            Console.WriteLine($"Finalized: {nameof(GroupedTableFooterView)}");
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            _label.Text = $"Custom DataContext for section: {ViewModel.Id}";
        }
    }
}
