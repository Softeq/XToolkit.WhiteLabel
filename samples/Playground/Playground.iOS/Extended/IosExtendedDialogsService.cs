// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Playground.Extended;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.Services;

namespace Playground.iOS.Extended
{
    public class IosExtendedDialogsService : StoryboardDialogsService, IExtendedDialogsService
    {
        private readonly IViewLocator _viewLocator;

        public IosExtendedDialogsService(IViewLocator viewLocator, ILogManager logManager, IContainer container)
            : base(viewLocator, logManager, container)
        {
            _viewLocator = viewLocator;
        }

        public Task<DateTime> ShowDialogAsync(ChooseBetterDateDialogConfig config)
        {
            return new IosChooseBetterDateDialog(_viewLocator, config).ShowAsync();
        }
    }
}
