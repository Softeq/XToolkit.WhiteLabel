// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Validation;
using Softeq.XToolkit.WhiteLabel.Validation.Rules;

namespace Playground.Forms.ViewModels.Components
{
    public class ValidationPageViewModel : ViewModelBase
    {
        private readonly ValidatableGroup _validationGroup;

        public ValidationPageViewModel()
        {
            UserName = new ValidatableObject<string>();
            UserName.AddRule(new StringNotEmptyRule("A username is required."));
            UserName.AddRule(new StringLengthRule(3, 10, "Username must be 3-10 characters."));

            Email = new ValidatableObject<string>();
            Email.AddRule(new StringNotEmptyRule("Email is required."));
            Email.AddRule(new EmailRule("Invalid Email."));

            TermsAndCondition = new ValidatableObject<bool>();
            TermsAndCondition.AddRule(new IsValueTrueRule("Please accept terms and condition"));

            _validationGroup = new ValidatableGroup(UserName, Email, TermsAndCondition);

            CheckNameCommand = new RelayCommand(CheckName);
            SubmitCommand = new AsyncCommand(SubmitAsync);
        }

        public ValidatableObject<string> UserName { get; }

        public ValidatableObject<string> Email { get; }

        public ValidatableObject<bool> TermsAndCondition { get; }

        public ICommand CheckNameCommand { get; }

        public AsyncCommand SubmitCommand { get; }

        private void CheckName()
        {
            UserName.Validate();
        }

        private async Task SubmitAsync()
        {
            if (_validationGroup.Validate())
            {
                await Task.Delay(3000);

                UserName.Errors.Add("Server validation: Username is not valid");
            }
        }
    }
}
