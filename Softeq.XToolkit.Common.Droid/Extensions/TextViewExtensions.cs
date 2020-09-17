// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Widget;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:Android.Widget.TextView"/>.
    /// </summary>
    public static class TextViewExtensions
    {
        public static void HighlightStrings(
            this TextView textView,
            IEnumerable<(int Start, int Length)> ranges,
            Color color)
        {
            var text = textView.Text;
            var spannedString = new SpannableString(text);

            foreach (var (start, length) in ranges)
            {
                spannedString.SetSpan(
                    new ForegroundColorSpan(color),
                    start,
                    start + length,
                    SpanTypes.InclusiveInclusive);
            }

            textView.SetText(spannedString, TextView.BufferType.Spannable);
        }
    }
}
