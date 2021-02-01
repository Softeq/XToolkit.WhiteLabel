// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Forms.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ListViewNoHighlightEffect = Playground.Forms.iOS.Effects.ListViewNoHighlightEffect;

[assembly: ResolutionGroupName(Effects.GroupName)]
[assembly: ExportEffect(typeof(ListViewNoHighlightEffect), nameof(ListViewNoHighlightEffect))]

namespace Playground.Forms.iOS.Effects
{
    public class ListViewNoHighlightEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var listView = (UIKit.UITableView)Control;

            listView.AllowsSelection = false;
        }

        protected override void OnDetached()
        {
        }
    }
}
