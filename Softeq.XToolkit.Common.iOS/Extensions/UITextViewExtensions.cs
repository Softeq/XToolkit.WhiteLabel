// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.iOS.TextFilters;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:UIKit.UITextField"/> and <see cref="T:UIKit.UITextView"/>.
    /// </summary>
    public static class UITextViewExtensions
    {
        /// <summary>
        ///     Sets the filter that will be used for target view.
        /// </summary>
        /// <param name="textField">Target view.</param>
        /// <param name="filter">Filter to apply.</param>
        public static void SetFilter(this UITextField textField, ITextFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            textField.ShouldChangeCharacters = (field, range, replacementString) =>
            {
                return filter.ShouldChangeText(field, field.Text, range, replacementString);
            };
        }

        /// <summary>
        ///     Sets the filter that will be used for target view.
        /// </summary>
        /// <param name="textField">Target view.</param>
        /// <param name="filter">Filter to apply.</param>
        public static void SetFilter(this UITextView textField, ITextFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            textField.ShouldChangeText = (field, range, text) =>
            {
                return filter.ShouldChangeText(field, field.Text, range, text);
            };
        }
    }
}
