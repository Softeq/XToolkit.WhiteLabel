// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.Components;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Components
{
    [Activity]
    public class PushNotificationsPageActivity : ActivityBase<PushNotificationsPageViewModel>
    {
        private Button? _registerBtn;
        private Button? _unregisterBtn;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_push_notifications);

            _registerBtn = FindViewById<Button>(Resource.Id.activity_push_notifications_register_btn);
            _unregisterBtn = FindViewById<Button>(Resource.Id.activity_push_notifications_unregister_btn);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            _registerBtn?.SetCommand(ViewModel.RegisterCommand);
            _unregisterBtn?.SetCommand(ViewModel.UnregisterCommand);
        }
    }
}
