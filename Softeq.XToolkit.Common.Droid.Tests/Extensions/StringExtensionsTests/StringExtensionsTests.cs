// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Android.Text;
using Android.Text.Style;
using Softeq.XToolkit.Common.Droid.Extensions;
using Softeq.XToolkit.Common.Helpers;
using Xunit;
using Object = Java.Lang.Object;

namespace Softeq.XToolkit.Common.Droid.Tests.Extensions.StringExtensionsTests
{
    public class StringExtensionsTests
    {
        #region Null string

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableNullStringWithRangeAndSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnNullString_WithAnyTextRange_WithSpans_ThrowsCorrectException(
            TextRange textRange, Object[] spans)
        {
            var str = null as string;
            Assert.Throws<ArgumentNullException>(() => str!.FormatSpannable(textRange, spans));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableNullStringWithoutSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnNullString_WithAnyTextRange_WithoutSpans_ThrowsCorrectException(
            TextRange textRange)
        {
            var str = null as string;
            Assert.Throws<ArgumentNullException>(() => str!.FormatSpannable(textRange));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableNullStringWithSpansOnlyTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnNullString_WithoutTextRange_WithSpans_ThrowsCorrectException(
            Object[] spans)
        {
            var str = null as string;
            Assert.Throws<ArgumentNullException>(() => str!.FormatSpannable(spans));
        }

        [Fact]
        public void FormatSpannable_WhenCalledOnNullString_WithoutTextRange_WithoutSpans_ThrowsCorrectException()
        {
            var str = null as string;
            Assert.Throws<ArgumentNullException>(() => str!.FormatSpannable());
        }

        #endregion

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableStringsTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithNullTextRange_ThrowsCorrectException(
            string str)
        {
            var spans = new Object[] { };
            Assert.Throws<ArgumentNullException>(() => str.FormatSpannable(StringExtensionsDataProvider.NullTextRange, spans));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableWithNonNullTextRangeWithoutSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithNonNullTextRange_WithoutSpans_ThrowsCorrectException(
            string str, TextRange textRange)
        {
            var spans = new Object[] { };
            Assert.Throws<ArgumentException>(() => str.FormatSpannable(textRange, spans));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableWithIncorrectTextRangeWithSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithIncorrectTextRange_WithSpans_ThrowsCorrectException(
            string str, TextRange textRange, Object[] spans)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatSpannable(textRange, spans));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableWithCorrectTextRangeWithStyleSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithCorrectTextRange_WithStyleSpans_AppliesSpans(
            string str, TextRange textRange, Object[] spans)
        {
            var spannable = str.FormatSpannable(textRange, spans);
            var appliedSpans = GetSpans<StyleSpan>(spannable, textRange);

            AssertAppliedSpans(spans, appliedSpans);
            AssertNoSpansOutsideSpecifiedIntervalApplied<StyleSpan>(spannable, textRange);
        }

        [Theory]
        [MemberData(
           nameof(StringExtensionsDataProvider.FormatSpannableWithCorrectTextRangeWithForegroundColorSpansTestData),
           MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithCorrectTextRange_WithForegroundColorSpans_AppliesSpans(
           string str, TextRange textRange, Object[] spans)
        {
            var spannable = str.FormatSpannable(textRange, spans);
            var appliedSpans = GetSpans<ForegroundColorSpan>(spannable, textRange);

            AssertAppliedSpans(spans, appliedSpans);
            AssertNoSpansOutsideSpecifiedIntervalApplied<ForegroundColorSpan>(spannable, textRange);
        }

        [Theory]
        [MemberData(
           nameof(StringExtensionsDataProvider.FormatSpannableWithCorrectTextRangeWithDifferentSpansTestData),
           MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithCorrectTextRange_WithDifferentSpans_AppliesSpans(
           string str, TextRange textRange, Object[] spans)
        {
            var spannable = str.FormatSpannable(textRange, spans);
            var appliedStyleSpans = GetSpans<StyleSpan>(spannable, textRange);
            var appliedForegroundColorSpans = GetSpans<ForegroundColorSpan>(spannable, textRange);
            var appliedSpans = appliedStyleSpans.Concat(appliedForegroundColorSpans).ToArray();

            AssertAppliedSpans(spans, appliedSpans);
            AssertNoSpansOutsideSpecifiedIntervalApplied<StyleSpan>(spannable, textRange);
            AssertNoSpansOutsideSpecifiedIntervalApplied<ForegroundColorSpan>(spannable, textRange);
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableWithoutTextRangeWithStyleSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithoutTextRange_WithStyleSpans_AppliesSpans(
            string str, Object[] spans)
        {
            var spannable = str.FormatSpannable(spans);
            var appliedSpans = GetSpans<StyleSpan>(spannable, new TextRange(0, str.Length));

            AssertAppliedSpans(spans, appliedSpans);
        }

        [Theory]
        [MemberData(
           nameof(StringExtensionsDataProvider.FormatSpannableWithoutTextRangeWithForegroundColorSpansTestData),
           MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithoutTextRange_WithForegroundColorSpans_AppliesSpans(
            string str, Object[] spans)
        {
            var spannable = str.FormatSpannable(spans);
            var appliedSpans = GetSpans<ForegroundColorSpan>(spannable, new TextRange(0, str.Length));

            AssertAppliedSpans(spans, appliedSpans);
        }

        [Theory]
        [MemberData(
           nameof(StringExtensionsDataProvider.FormatSpannableWithoutTextRangeWithDifferentSpansTestData),
           MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithoutTextRange_WithDifferentSpans_AppliesSpans(
            string str, Object[] spans)
        {
            var spannable = str.FormatSpannable(spans);
            var appliedStyleSpans = GetSpans<StyleSpan>(spannable, new TextRange(0, str.Length));
            var appliedForegroundColorSpans = GetSpans<ForegroundColorSpan>(spannable, new TextRange(0, str.Length));
            var appliedSpans = appliedStyleSpans.Concat(appliedForegroundColorSpans).ToArray();

            AssertAppliedSpans(spans, appliedSpans);
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableStringsTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithoutTextRange_WithoutSpans_ThrowsCorrectException(
            string str)
        {
            Assert.Throws<ArgumentException>(() => str.FormatSpannable());
        }

        private void AssertAppliedSpans(Object[] spans, Object[] appliedSpans)
        {
            var nonNullSpans = spans.Where(t => t != null).Distinct();
            var nonNullAppliedSpans = appliedSpans.Where(t => t != null);

            Assert.Equal(nonNullSpans.Count(), nonNullAppliedSpans.Count());
            foreach (var span in nonNullSpans)
            {
                Assert.Contains(span, appliedSpans);
            }

            foreach (var appliedSpan in nonNullAppliedSpans)
            {
                Assert.Contains(appliedSpan, spans);
            }
        }

        private void AssertNoSpansOutsideSpecifiedIntervalApplied<T>(SpannableString? spannable, TextRange textRange)
        {
            int spannableLength = spannable?.Length() ?? 0;
            if (textRange.Position > 0)
            {
                var spansBefore = GetSpans<T>(spannable, new TextRange(0, textRange.Position - 1));
                Assert.Empty(spansBefore);
            }

            if (textRange.Position + textRange.Length < spannableLength - 1)
            {
                var spansAfter = GetSpans<T>(spannable, new TextRange(textRange.Position + textRange.Length + 1, spannableLength - 1));
                Assert.Empty(spansAfter);
            }
        }

        private Object[] GetSpans<T>(SpannableString? spannable, TextRange textRange)
        {
            return spannable?.GetSpans(
                textRange.Position,
                textRange.Position + textRange.Length,
                Java.Lang.Class.FromType(typeof(T))) ?? new Object[] { };
        }
    }
}
