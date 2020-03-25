// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.Services;

namespace Playground.iOS.Extended
{
    public class IosExtendedDialogsService : StoryboardDialogsService
    {
        private readonly IViewLocator _viewLocator;

        public IosExtendedDialogsService(IViewLocator viewLocator, ILogManager logManager, IContainer container)
            : base(viewLocator, logManager, container)
        {
            _viewLocator = viewLocator;
        }

        public override Task<T> ShowDialogAsync<T>(IDialogConfig<T> config)
        {
            return config switch
            {
                ChooseBetterDateDialogConfig bestConfig =>
                    new IosChooseBetterDateDialog(_viewLocator, bestConfig).ShowAsync<T>(),

                _ => base.ShowDialogAsync(config)
            };
        }
    }
}
