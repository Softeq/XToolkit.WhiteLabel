// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using System.Threading.Tasks;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;

namespace Playground.iOS.Extended
{
    public class IosChooseBetterDateDialog : AlertViewControllerBase, IDialog<DateTime>
    {
        private readonly ChooseBetterDateDialogConfig _config;

        public IosChooseBetterDateDialog(IViewLocator viewLocator, ChooseBetterDateDialogConfig config)
            : base(viewLocator)
        {
            _config = config;

            Title = config.Title;
        }

        public Task<DateTime> ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<DateTime>();

            AddAction(
                AlertAction.Default(
                    _config.First.ToString(CultureInfo.CurrentCulture),
                    () => dialogResult.TrySetResult(_config.First)));

            AddAction(
                AlertAction.Default(
                    _config.Second.ToString(CultureInfo.CurrentCulture),
                    () => dialogResult.TrySetResult(_config.Second)));

            Present();

            return dialogResult.Task;
        }
    }
}
