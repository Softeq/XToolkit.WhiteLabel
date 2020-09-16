// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Text;
using Android.Widget;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:Android.Widget.EditText"/>.
    /// </summary>
    public static class EditTextExtensions
    {
        /// <summary>
        ///     Allows to move cursor to the end of text in <see cref="T:Android.Widget.EditText" /> when it gains focus.
        /// </summary>
        /// <param name="editText"><see cref="T:Android.Widget.EditText" /> control.</param>
        public static void KeepFocusAtTheEndOfField(this EditText editText)
        {
            editText.FocusChange += (sender, e) =>
            {
                if (editText.Text != null)
                {
                    editText.SetSelection(editText.Text.Length);
                }
            };
        }

        /// <summary>
        ///     Allows applying multiple input filters to the <see cref="T:Android.Widget.EditText" />.
        /// </summary>
        /// <param name="editText"><see cref="T:Android.Widget.EditText" /> control.</param>
        /// <param name="filters">Array of filters.</param>
        public static void SetFilters(this EditText editText, params IInputFilter[] filters)
        {
            editText.SetFilters(filters);
        }
    }
}
