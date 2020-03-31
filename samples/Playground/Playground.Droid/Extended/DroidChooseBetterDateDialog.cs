// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Playground.Droid.Extended
{
    public class DroidChooseBetterDateDialog : AlertDialogBase, IDialog<DateTime>
    {
        private readonly ChooseBetterDateDialogConfig _config;

        public DroidChooseBetterDateDialog(ChooseBetterDateDialogConfig config)
        {
            _config = config;
        }

        public Task<DateTime> ShowAsync()
        {
            var dialogResult = new TaskCompletionSource<DateTime>();

            Execute.BeginOnUIThread(() =>
            {
                var builder = GetBuilder()
                    .SetTitle(_config.Title);

                var options = new[]
                {
                    _config.First,
                    _config.Second
                };

                var items = options.Select(x => x.ToString(CultureInfo.CurrentCulture)).ToArray();
                builder.SetItems(items, (_, args) =>
                {
                    dialogResult.TrySetResult(options[args.Which]);
                });

                Present(builder);
            });

            return dialogResult.Task;
        }
    }
}
