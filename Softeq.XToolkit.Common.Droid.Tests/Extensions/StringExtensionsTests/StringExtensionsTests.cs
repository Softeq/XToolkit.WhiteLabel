// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Android.Text;
using Android.Text.Style;
using Softeq.XToolkit.Common.Droid.Extensions;
using Xunit;
using Object = Java.Lang.Object;

namespace Softeq.XToolkit.Common.Droid.Tests.Extensions.ContextExtensionsTests
{
    public class StringExtensionsTests
    {
        #region Null string

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableNullStringWithSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnNullString_WithStartingIndexAndLength_WithSpans_ThrowsCorrectException(
            int startingIndex, int length, Object[] spans)
        {
            var str = null as string;
            Assert.Throws<ArgumentNullException>(() => str!.FormatSpannable(startingIndex, length, spans));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableNullStringWithoutSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnNullString_WithStartingIndexAndLength_WithoutSpans_ThrowsCorrectException(
            int startingIndex, int length)
        {
            var str = null as string;
            Assert.Throws<ArgumentNullException>(() => str!.FormatSpannable(startingIndex, length));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableNullStringWithSpansOnlyTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnNullString_WithoutStartingIndexAndLength_WithSpans_ThrowsCorrectException(
            Object[] spans)
        {
            var str = null as string;
            Assert.Throws<ArgumentNullException>(() => str!.FormatSpannable(spans));
        }

        [Fact]
        public void FormatSpannable_WhenCalledOnNullString_WithoutStartingIndexAndLength_WithoutSpans_ThrowsCorrectException()
        {
            var str = null as string;
            Assert.Throws<ArgumentNullException>(() => str!.FormatSpannable());
        }

        #endregion

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableWithAnyStartingIndexOrLengthWithoutSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithAnyStartingIndexAndLength_WithoutSpans_ThrowsCorrectException(
            string str, int startingIndex, int length)
        {
            var spans = new Object[] { };
            Assert.Throws<ArgumentException>(() => str.FormatSpannable(startingIndex, length, spans));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableWithIncorrectStartingIndexOrLengthWithSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithIncorrectStartingIndexOrLength_WithSpans_ThrowsCorrectException(
            string str, int startingIndex, int length, Object[] spans)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatSpannable(startingIndex, length, spans));
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableWithCorrectStartingIndexAndLengthWithStyleSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithCorrectStartingIndexAndLength_WithStyleSpans_AppliesSpans(
            string str, int startingIndex, int length, Object[] spans)
        {
            var spannable = str.FormatSpannable(startingIndex, length, spans);
            var appliedSpans = GetSpans<StyleSpan>(spannable, startingIndex, length);

            AssertAppliedSpans(spans, appliedSpans);
            AssertNoSpansOutsideSpecifiedIntervalApplied<StyleSpan>(spannable, startingIndex, length);
        }

        [Theory]
        [MemberData(
           nameof(StringExtensionsDataProvider.FormatSpannableWithCorrectStartingIndexAndLengthWithForegroundColorSpansTestData),
           MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithCorrectStartingIndexAndLength_WithForegroundColorSpans_AppliesSpans(
           string str, int startingIndex, int length, Object[] spans)
        {
            var spannable = str.FormatSpannable(startingIndex, length, spans);
            var appliedSpans = GetSpans<ForegroundColorSpan>(spannable, startingIndex, length);

            AssertAppliedSpans(spans, appliedSpans);
            AssertNoSpansOutsideSpecifiedIntervalApplied<ForegroundColorSpan>(spannable, startingIndex, length);
        }

        [Theory]
        [MemberData(
           nameof(StringExtensionsDataProvider.FormatSpannableWithCorrectStartingIndexAndLengthWithDifferentSpansTestData),
           MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithCorrectStartingIndexAndLength_WithDifferentSpans_AppliesSpans(
           string str, int startingIndex, int length, Object[] spans)
        {
            var spannable = str.FormatSpannable(startingIndex, length, spans);
            var appliedStyleSpans = GetSpans<StyleSpan>(spannable, startingIndex, length);
            var appliedForegroundColorSpans = GetSpans<ForegroundColorSpan>(spannable, startingIndex, length);
            var appliedSpans = appliedStyleSpans.Concat(appliedForegroundColorSpans).ToArray();

            AssertAppliedSpans(spans, appliedSpans);
            AssertNoSpansOutsideSpecifiedIntervalApplied<StyleSpan>(spannable, startingIndex, length);
            AssertNoSpansOutsideSpecifiedIntervalApplied<ForegroundColorSpan>(spannable, startingIndex, length);
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableWithoutStartingIndexAndLengthWithStyleSpansTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithoutStartingIndexAndLength_WithStyleSpans_AppliesSpans(
            string str, Object[] spans)
        {
            var spannable = str.FormatSpannable(spans);
            var appliedSpans = GetSpans<StyleSpan>(spannable, 0, str.Length);

            AssertAppliedSpans(spans, appliedSpans);
        }

        [Theory]
        [MemberData(
           nameof(StringExtensionsDataProvider.FormatSpannableWithoutStartingIndexAndLengthWithForegroundColorSpansTestData),
           MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithoutStartingIndexAndLength_WithForegroundColorSpans_AppliesSpans(
            string str, Object[] spans)
        {
            var spannable = str.FormatSpannable(spans);
            var appliedSpans = GetSpans<ForegroundColorSpan>(spannable, 0, str.Length);

            AssertAppliedSpans(spans, appliedSpans);
        }

        [Theory]
        [MemberData(
           nameof(StringExtensionsDataProvider.FormatSpannableWithoutStartingIndexAndLengthWithDifferentSpansTestData),
           MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithoutStartingIndexAndLength_WithDifferentSpans_AppliesSpans(
            string str, Object[] spans)
        {
            var spannable = str.FormatSpannable(spans);
            var appliedStyleSpans = GetSpans<StyleSpan>(spannable, 0, str.Length);
            var appliedForegroundColorSpans = GetSpans<ForegroundColorSpan>(spannable, 0, str.Length);
            var appliedSpans = appliedStyleSpans.Concat(appliedForegroundColorSpans).ToArray();

            AssertAppliedSpans(spans, appliedSpans);
        }

        [Theory]
        [MemberData(
            nameof(StringExtensionsDataProvider.FormatSpannableStringsTestData),
            MemberType = typeof(StringExtensionsDataProvider))]
        public void FormatSpannable_WhenCalledOnCorrectString_WithoutStartingIndexAndLength_WithoutSpans_ThrowsCorrectException(
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

        private void AssertNoSpansOutsideSpecifiedIntervalApplied<T>(SpannableString? spannable, int startingIndex, int length)
        {
            int spannableLength = spannable?.Length() ?? 0;
            if (startingIndex > 0)
            {
                var spansBefore = GetSpans<T>(spannable, 0, startingIndex - 1);
                Assert.Empty(spansBefore);
            }

            if (startingIndex + length < spannableLength - 1)
            {
                var spansAfter = GetSpans<T>(spannable, startingIndex + length + 1, spannableLength - 1);
                Assert.Empty(spansAfter);
            }
        }

        private Object[] GetSpans<T>(SpannableString? spannable, int startingIndex, int length)
        {
            return spannable?.GetSpans(startingIndex, startingIndex + length, Java.Lang.Class.FromType(typeof(T))) ?? new Object[] { };
        }
    }
}
