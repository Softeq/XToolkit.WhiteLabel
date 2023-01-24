// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Playground.ViewModels.TestApproach
{
    public interface ICommonNavigationService
    {
        Task<string> NavigateToEditViewModel(string name);
        void NavigateToProvideNameFlow();
    }
}
