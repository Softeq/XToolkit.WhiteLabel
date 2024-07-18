// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.Components;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Components
{
    [Activity(Label = "Connectivity")]
    public class ConnectivityPageActivity : ActivityBase<ConnectivityPageViewModel>
    {
        private TextView? _connectionTextView;
        private TextView? _typesTextView;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_connectivity);

            _connectionTextView = FindViewById<TextView>(Resource.Id.textViewConnection);
            _typesTextView = FindViewById<TextView>(Resource.Id.textViewTypes);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.ConnectionStatus, () => _connectionTextView!.Text);
            this.Bind(() => ViewModel.ConnectionProfiles, () => _typesTextView!.Text);
        }
    }
}
