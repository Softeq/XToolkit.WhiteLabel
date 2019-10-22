using System;
using Playground.iOS.Views;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Dialogs
{
    public partial class SelectPresentationStyleViewController : ViewControllerBase<SelectPresentationStyleViewModel>
    {
        public SelectPresentationStyleViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Table.RegisterClassForCellReuse(typeof(SimpleTableViewCell), nameof(SimpleTableViewCell));
            Table.Source = new BindableTableViewSource<string, SimpleTableViewCell>(Table, ViewModel.StyleIds)
            {
                HeightForRow = 60,
                ItemClick = ViewModel.SelectItemCommand
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

