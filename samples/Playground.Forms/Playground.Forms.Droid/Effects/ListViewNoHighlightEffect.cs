// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Widget;
using Playground.Forms.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ListViewNoHighlightEffect = Playground.Forms.Droid.Effects.ListViewNoHighlightEffect;

[assembly: ResolutionGroupName(Effects.GroupName)]
[assembly: ExportEffect(typeof(ListViewNoHighlightEffect), nameof(ListViewNoHighlightEffect))]

namespace Playground.Forms.Droid.Effects
{
    public class ListViewNoHighlightEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var listView = (Android.Widget.ListView)Control;

            listView.ChoiceMode = ChoiceMode.None;
            listView.SetSelector(Android.Resource.Color.Transparent);
        }

        protected override void OnDetached()
        {
        }
    }
}
