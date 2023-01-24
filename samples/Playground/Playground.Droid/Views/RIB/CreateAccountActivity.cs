// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.RIB.CreateAccount;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.RIB
{
    [Activity(Label = "")]
    [SuppressMessage("ReSharper", "RedundantOverriddenMember", Justification = "Just for play.")]
    public class CreateAccountActivity : ActivityBase<CreateAccountViewModel>
    {
        private EditText _usernameEditText;
        private EditText _passwordEditText;
        private EditText _repeatPasswordEditText;

        private Button _registerButton;
        private Button _closeCreateAccountButton;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_register);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _usernameEditText = FindViewById<EditText>(Resource.Id.username_edittext);
            _passwordEditText = FindViewById<EditText>(Resource.Id.password_edittext);
            _repeatPasswordEditText = FindViewById<EditText>(Resource.Id.repeat_password_edittext);

            _registerButton = FindViewById<Button>(Resource.Id.register_button);
            _registerButton.SetCommand(ViewModel.RegisterCommand);

            _closeCreateAccountButton = FindViewById<Button>(Resource.Id.close_create_account_button);
            _closeCreateAccountButton.SetCommand(ViewModel.CloseCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Name, () => _usernameEditText.Text, BindingMode.TwoWay);
            this.Bind(() => ViewModel.Password, () => _passwordEditText.Text, BindingMode.TwoWay);
            this.Bind(() => ViewModel.RepeatPassword, () => _repeatPasswordEditText.Text, BindingMode.TwoWay);
        }
    }
}
