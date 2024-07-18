// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using ObjCRuntime;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Playground.iOS.Views
{
    public partial class MainPageItemViewCell : BindableTableViewCell<CommandAction>
    {
        public static readonly NSString Key = new NSString(nameof(MainPageItemViewCell));
        public static readonly UINib Nib;

        static MainPageItemViewCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected MainPageItemViewCell(NativeHandle handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            NameLabel.Text = ViewModel.Title;
        }
    }
}
