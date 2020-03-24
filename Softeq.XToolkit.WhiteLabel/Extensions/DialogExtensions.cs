// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Dialogs;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class DialogExtensions
    {
        /// <summary>
        ///     Type-matching for return explicit result.
        /// </summary>
        /// <param name="dialog">An instance of <see cref="IDialog{TResult}"/>.</param>
        /// <typeparam name="TResult">Type of dialog result.</typeparam>
        /// <returns></returns>
        public static Task<TResult> ShowAsync<TResult>(this IDialog dialog)
        {
            return dialog switch
            {
                null => throw new ArgumentNullException(nameof(dialog)),

                IDialog<TResult> dialogWithResult => dialogWithResult.ShowAsync(),

                _ => throw new InvalidCastException(
                    $"Types inconsistency between `{dialog.GetType()}` and config `{typeof(IDialogConfig<TResult>)}`")
            };
        }
    }
}
