using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IInternalNavigationService
    {
        void NavigateToViewModel<T>(bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel> parameters = null) where T : IViewModelBase;
    }
}