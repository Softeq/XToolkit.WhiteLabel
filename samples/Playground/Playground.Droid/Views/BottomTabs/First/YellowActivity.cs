// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Widget;
using Playground.Converters;
using Playground.ViewModels.BottomTabs.First;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.BottomTabs.First
{
    [Activity]
    public class YellowActivity : ActivityBase<YellowViewModel>
    {
        private Button _incrementButton = null!;
        private TextView _incrementLabel = null!;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_yellow);

            _incrementLabel = FindViewById<TextView>(Resource.Id.textView1)!;

            _incrementButton = FindViewById<Button>(Resource.Id.button2)!;
            _incrementButton.SetCommand(ViewModel.IncrementCommand);
            _incrementButton.Click += (sender, e) => _incrementLabel.Text = "changed";
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Count, () => _incrementButton.Text, IntToStringConverter.Instance);
        }
    }
}
