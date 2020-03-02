// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Forms.Navigation;

namespace Playground.Forms.Services
{
    public class PlaygroundViewLocator : FormsViewLocator
    {
        protected override string BuildPageTypeName(string viewModelTypeName)
        {
            var name = viewModelTypeName
                .Replace(".ViewModels.", ".Views.")
                .Replace("ViewModel", string.Empty);
            return name;
        }
    }
}
