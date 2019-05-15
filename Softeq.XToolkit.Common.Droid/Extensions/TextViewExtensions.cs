// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Widget;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    public static class TextViewExtensions
    {
        public static void HighlightStrings(this TextView textView,
            IEnumerable<(int Start, int Length)> ranges,
            Color color)
        {
            var text = textView.Text;
            var spannedString = new SpannableString(text);

            foreach (var (Start, Length) in ranges)
            {
                spannedString.SetSpan(
                    new ForegroundColorSpan(color),
                    Start,
                    Start + Length,
                    SpanTypes.InclusiveInclusive);
            }

            textView.SetText(spannedString, TextView.BufferType.Spannable);
        }
    }
}
