using System;
using Softeq.XToolkit.Bindings.iOS.Bindable;

namespace Playground.iOS.Views
{
    public class SimpleTableViewCell : BindableTableViewCell<string>
    {
        public SimpleTableViewCell(IntPtr handle) : base(handle)
        {

        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();
            TextLabel.Text = ViewModel;
        }
    }
}
