// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.Converters;
using Playground.ViewModels.BottomTabs.First;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.BottomTabs.Second
{
    public class RedFragment : FragmentBase<RedViewModel>
    {
        private Button _navigateButton;
        private Button _incrementButton;
        private TextView _incrementLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_red, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _navigateButton = view.FindViewById<Button>(Resource.Id.button1);
            _navigateButton.SetCommand(ViewModel.NavigateCommand);

            _incrementLabel = view.FindViewById<TextView>(Resource.Id.textView1);

            _incrementButton = view.FindViewById<Button>(Resource.Id.button2);
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
