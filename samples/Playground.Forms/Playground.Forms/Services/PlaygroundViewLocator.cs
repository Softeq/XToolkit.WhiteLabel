// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Forms.Navigation;

namespace Playground.Forms.Services
{
    public class PlaygroundViewLocator : FormsViewLocator
    {
        protected override string BuildViewTypeName(string viewModelTypeName)
        {
            return viewModelTypeName.Replace(".ViewModels.", ".Views.");
        }
    }
}
