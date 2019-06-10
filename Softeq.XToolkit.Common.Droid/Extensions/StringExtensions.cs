// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Text;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    public static class StringExtensions
    {
        public static SpannableString FormatSpannable(this string unformattedString, params Java.Lang.Object[] spans)
        {
            var formattedString = new SpannableString(unformattedString);
            foreach (var span in spans)
            {
                formattedString.SetSpan(span, 0, unformattedString.Length, SpanTypes.ExclusiveExclusive);
            }

            return formattedString;
        }

        public static SpannableString FormatSpannable(this string unformattedString, int startingIndex, int length, params Java.Lang.Object[] spans)
        {
            startingIndex = startingIndex < 0 ? 0 : startingIndex;
            length = startingIndex + length >= unformattedString.Length
                    ? unformattedString.Length - startingIndex - 1 : length;

            var formattedString = new SpannableString(unformattedString);
            foreach (var span in spans)
            {
                formattedString.SetSpan(span, startingIndex, length, SpanTypes.ExclusiveExclusive);
            }

            return formattedString;
        }
    }
}
