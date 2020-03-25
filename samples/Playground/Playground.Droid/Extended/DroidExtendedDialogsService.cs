// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Extensions;

namespace Playground.Droid.Extended
{
    public class DroidExtendedDialogsService : DroidFragmentDialogService
    {
        public DroidExtendedDialogsService(IViewLocator viewLocator, IAlertBuilder alertBuilder, IContainer iocContainer)
            : base(viewLocator, alertBuilder, iocContainer)
        {
        }

        public override Task<T> ShowDialogAsync<T>(IDialogConfig<T> config)
        {
            return config switch
            {
                ChooseBetterDateDialogConfig bestConfig => new DroidChooseBetterDateDialog(bestConfig).ShowAsync<T>(),
                _ => base.ShowDialogAsync(config)
            };
        }
    }
}
