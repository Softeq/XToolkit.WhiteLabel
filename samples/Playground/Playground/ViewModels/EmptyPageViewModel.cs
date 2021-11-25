// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels
{
    [SuppressMessage("ReSharper", "EmptyDestructor", Justification = "Just for play.")]
    [SuppressMessage("ReSharper", "RedundantOverriddenMember", Justification = "Just for play.")]
    [SuppressMessage("ReSharper", "EmptyConstructor", Justification = "Just for play.")]
    public class EmptyPageViewModel : ViewModelBase
    {
        public EmptyPageViewModel()
        {
            // Put your code HERE.
        }

        ~EmptyPageViewModel()
        {
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            // Put your code HERE.
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            // Put your code HERE.
        }
    }
}
