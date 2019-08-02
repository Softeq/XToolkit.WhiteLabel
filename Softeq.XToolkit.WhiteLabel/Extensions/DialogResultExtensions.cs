// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class DialogResultExtensions
    {
        public static async Task<T> WaitUntilDismissed<T>(this Task<IDialogResult<T>> dialogTask)
        {
            var dialogResult = await dialogTask;
            await dialogResult.WaitDismissAsync;
            return dialogResult.Value;
        }

        public static async Task WaitUntilDismissed(this Task<IDialogResult> dialogTask)
        {
            var dialogResult = await dialogTask;
            await dialogResult.WaitDismissAsync;
        }
    }
}
