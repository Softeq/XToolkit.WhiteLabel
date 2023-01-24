using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.RIB.Login;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.RIB
{
    [Activity(Label = "")]
    [SuppressMessage("ReSharper", "RedundantOverriddenMember", Justification = "Just for play.")]
    public class LoginActivity : ActivityBase<LoginViewModel>
    {
        private EditText _usernameEditText;
        private EditText _passwordEditText;
        private Button _loginButton;
        private Button _createAccountButton;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_login);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _usernameEditText = FindViewById<EditText>(Resource.Id.username_edittext);
            _passwordEditText = FindViewById<EditText>(Resource.Id.password_edittext);

            _loginButton = FindViewById<Button>(Resource.Id.login_button);
            _loginButton.SetCommand(ViewModel.LoginCommand);

            _createAccountButton = FindViewById<Button>(Resource.Id.create_account_button);
            _createAccountButton.SetCommand(ViewModel.CreateAccountCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Name, () => _usernameEditText.Text, BindingMode.TwoWay);
            this.Bind(() => ViewModel.Password, () => _passwordEditText.Text, BindingMode.TwoWay);
        }
    }
}
