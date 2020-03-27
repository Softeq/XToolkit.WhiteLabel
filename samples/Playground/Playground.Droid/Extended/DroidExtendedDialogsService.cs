// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Playground.Extended;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;

namespace Playground.Droid.Extended
{
    public class DroidExtendedDialogsService : DroidFragmentDialogService, IExtendedDialogsService
    {
        public DroidExtendedDialogsService(IViewLocator viewLocator, IContainer container)
            : base(viewLocator, container)
        {
        }

        public Task<DateTime> ShowDialogAsync(ChooseBetterDateDialogConfig config)
        {
            return new DroidChooseBetterDateDialog(config).ShowAsync();
        }
    }
}
