// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.RIB.Platform.Navigation
{
    public interface IRibNavigationService
    {
        void GoBack();
        public void NavigateToViewModel(ViewModelBase viewModel, IReadOnlyList<NavigationParameterModel> parameters = null);
    }
}
