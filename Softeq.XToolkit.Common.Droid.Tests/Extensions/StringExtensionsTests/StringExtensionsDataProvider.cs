// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Text.Style;
using Xunit;
using Object = Java.Lang.Object;
using String = Java.Lang.String;

namespace Softeq.XToolkit.Common.Droid.Tests.Extensions.ContextExtensionsTests
{
    internal static class StringExtensionsDataProvider
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        private static readonly Object NullObject = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        private static readonly string TestString1 = "abcd";
        private static readonly string TestString2 = "abcdefghijk";
        private static readonly string TestStringWithSingleChar = "a";

        private static readonly StyleSpan StyleSpan1 = new StyleSpan(Android.Graphics.TypefaceStyle.Bold);
        private static readonly StyleSpan StyleSpan2 = new StyleSpan(Android.Graphics.TypefaceStyle.Italic);
        private static readonly StyleSpan StyleSpan3 = new StyleSpan(Android.Graphics.TypefaceStyle.Normal);

        private static readonly ForegroundColorSpan ForegroundColorSpan1 = new ForegroundColorSpan(Android.Graphics.Color.White);
        private static readonly ForegroundColorSpan ForegroundColorSpan2 = new ForegroundColorSpan(Android.Graphics.Color.Transparent);
        private static readonly ForegroundColorSpan ForegroundColorSpan3 = new ForegroundColorSpan(Android.Graphics.Color.Red);

        public static TheoryData<string> FormatSpannableStringsTestData
           => new TheoryData<string>
           {
               { string.Empty },
               { TestString1 },
               { TestString2 },
               { TestStringWithSingleChar },
           };

        public static TheoryData<int, int, Object[]> FormatSpannableNullStringWithSpansTestData
           => new TheoryData<int, int, Object[]>
           {
               { 0, 0, new Object[] { } },
               { 0, 0, new Object[] { StyleSpan1 } },
               { 0, 0, new Object[] { StyleSpan1, ForegroundColorSpan1, StyleSpan2 } },
               { 1, 5, new Object[] { } },
               { 1, 5, new Object[] { ForegroundColorSpan2 } },
               { 1, 5, new Object[] { ForegroundColorSpan2, StyleSpan2, ForegroundColorSpan3 } },
               { 1, -2, new Object[] { NullObject } },
               { 1, -2, new Object[] { new String() } },
               { 1, -2, new Object[] { StyleSpan1, new String(), NullObject } },
               { -2, 0, new Object[] { } },
               { -2, 0, new Object[] { ForegroundColorSpan3 } },
               { -2, 0, new Object[] { StyleSpan3, NullObject, StyleSpan2 } },
               { -2, 5, new Object[] { } },
               { -2, 5, new Object[] { ForegroundColorSpan2 } },
               { -2, 5, new Object[] { StyleSpan3, NullObject, StyleSpan1 } },
               { -2, -2, new Object[] { NullObject } },
               { -2, -2, new Object[] { StyleSpan3 } },
               { -2, -2, new Object[] { ForegroundColorSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
           };

        public static TheoryData<int, int> FormatSpannableNullStringWithoutSpansTestData
           => new TheoryData<int, int>
           {
               { 0, 0 },
               { 1, 5 },
               { 1, -2 },
               { -2, 0 },
               { -2, 5 },
               { -2, -2 },
           };

        public static TheoryData<Object[]> FormatSpannableNullStringWithSpansOnlyTestData
           => new TheoryData<Object[]>
           {
               { new Object[] { } },
               { new Object[] { NullObject } },
               { new Object[] { StyleSpan1 } },
               { new Object[] { StyleSpan1, NullObject } },
               { new Object[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { new Object[] { StyleSpan1, ForegroundColorSpan1, ForegroundColorSpan1 } },
               { new Object[] { StyleSpan1, new String() } },
               { new Object[] { StyleSpan1, NullObject, new String(), ForegroundColorSpan1 } },
               { new Object[] { StyleSpan1, NullObject, new String(), StyleSpan1 } },
           };

        public static TheoryData<string, int, int> FormatSpannableWithAnyStartingIndexOrLengthWithoutSpansTestData
            => new TheoryData<string, int, int>
            {
                // incorrect
               { TestString1, 0, 0 },
               { TestString1, 0, -1 },
               { TestString1, 0, TestString1.Length + 1 },
               { TestString1, 2, 0 },
               { TestString1, 2, TestString1.Length-1 },
               { TestString1, 2, -1 },
               { TestString1, 2, TestString1.Length },
               { TestString1, 2, TestString1.Length + 1 },
               { TestString1, -1, 0 },
               { TestString1, -1, TestString1.Length-1 },
               { TestString1, -1, -1 },
               { TestString1, -1, TestString1.Length },
               { TestString1, -1, TestString1.Length + 1 },
               { TestString1, TestString1.Length, 0 },
               { TestString1, TestString1.Length, TestString1.Length-1 },
               { TestString1, TestString1.Length, -1 },
               { TestString1, TestString1.Length, TestString1.Length },
               { TestString1, TestString1.Length, TestString1.Length + 1 },
               { TestString1, TestString1.Length + 1, 0 },
               { TestString1, TestString1.Length + 1, TestString1.Length-1 },
               { TestString1, TestString1.Length + 1, -1 },
               { TestString1, TestString1.Length + 1, TestString1.Length },
               { TestString1, TestString1.Length + 1, TestString1.Length + 1 },
               // correct
               { TestString1, 0, 1 },
               { TestString1, 0, TestString1.Length },
               { TestString1, 2, 1 },
               { TestString1, 2, TestString1.Length-2 },
               { TestString1, TestString1.Length-1, 1 },
               { TestString2, 0, 1 },
               { TestString2, 0, TestString2.Length },
               { TestString2, 2, 1 },
               { TestString2, 2, TestString2.Length-2 },
               { TestString2, TestString2.Length-1, 1 },
               { TestStringWithSingleChar, 0, 1 },
            };

        public static TheoryData<string, int, int, Object[]> FormatSpannableWithIncorrectStartingIndexOrLengthWithSpansTestData
            => new TheoryData<string, int, int, Object[]>
            {
               { TestString1, 0, 0, new Object[] { new String() } },
               { TestString1, 0, -1, new Object[] { StyleSpan1, NullObject, new String() } },
               { TestString1, 0, TestString1.Length + 1, new Object[] { StyleSpan1, StyleSpan2 } },
               { TestString1, 2, 0, new Object[] { NullObject, ForegroundColorSpan1, ForegroundColorSpan2 } },
               { TestString1, 2, TestString1.Length-1, new Object[] { ForegroundColorSpan2 } },
               { TestString1, 2, -1, new Object[] { new String(), new String(), ForegroundColorSpan2 } },
               { TestString1, 2, TestString1.Length, new Object[] { StyleSpan3 } },
               { TestString1, 2, TestString1.Length + 1, new Object[] { StyleSpan2, StyleSpan3 } },
               { TestString1, -1, 0, new Object[] { ForegroundColorSpan1, new String() } },
               { TestString1, -1, TestString1.Length-1, new Object[] { ForegroundColorSpan3, ForegroundColorSpan3 } },
               { TestString1, -1, -1, new Object[] { ForegroundColorSpan3 } },
               { TestString1, -1, TestString1.Length, new Object[] { new String(), StyleSpan2 } },
               { TestString1, -1, TestString1.Length + 1, new Object[] { StyleSpan1, ForegroundColorSpan1 } },
               { TestString1, TestString1.Length, 0, new Object[] { NullObject, StyleSpan3 } },
               { TestString1, TestString1.Length, TestString1.Length-1, new Object[] { StyleSpan3, NullObject } },
               { TestString1, TestString1.Length, -1, new Object[] { StyleSpan1, StyleSpan1, StyleSpan3 } },
               { TestString1, TestString1.Length, TestString1.Length, new Object[] { ForegroundColorSpan1, NullObject } },
               { TestString1, TestString1.Length, TestString1.Length + 1, new Object[] { new String(), NullObject } },
               { TestString1, TestString1.Length + 1, 0, new Object[] { ForegroundColorSpan1, new String() } },
               { TestString1, TestString1.Length + 1, TestString1.Length-1, new Object[] { new String(), StyleSpan2 } },
               { TestString1, TestString1.Length + 1, -1, new Object[] { NullObject } },
               { TestString1, TestString1.Length + 1, TestString1.Length, new Object[] { StyleSpan1, NullObject, new String() } },
               { TestString1, TestString1.Length + 1, TestString1.Length + 1, new Object[] { ForegroundColorSpan2 } },
            };

        public static TheoryData<string, int, int, Object[]> FormatSpannableWithCorrectStartingIndexAndLengthWithStyleSpansTestData
            => new TheoryData<string, int, int, Object[]>
            {
               { TestString1, 0, 1, new Object[] { StyleSpan2, NullObject } },
               { TestString1, 0, TestString1.Length, new Object[] { StyleSpan1 } },
               { TestString1, 2, 1, new Object[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { TestString1, 2, TestString1.Length-2, new Object[] { StyleSpan2, StyleSpan3 } },
               { TestString1, TestString1.Length-1, 1, new Object[] { NullObject } },
               { TestString2, 0, 1, new Object[] { StyleSpan2 } },
               { TestString2, 0, TestString2.Length, new Object[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { TestString2, 2, 1, new Object[] { StyleSpan1, StyleSpan2 } },
               { TestString2, 2, TestString2.Length-2, new Object[] { StyleSpan1, StyleSpan1, StyleSpan1 } },
               { TestString2, TestString2.Length-1, 1, new Object[] { StyleSpan1, NullObject } },
               { TestStringWithSingleChar, 0, 1, new Object[] { StyleSpan3, StyleSpan2, StyleSpan3, NullObject } },
            };

        public static TheoryData<string, int, int, Object[]> FormatSpannableWithCorrectStartingIndexAndLengthWithForegroundColorSpansTestData
            => new TheoryData<string, int, int, Object[]>
            {
               { TestString1, 0, 1, new Object[] { ForegroundColorSpan1 } },
               { TestString1, 0, TestString1.Length, new Object[] { ForegroundColorSpan1 } },
               { TestString1, 2, 1, new Object[] { ForegroundColorSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString1, 2, TestString1.Length-2, new Object[] { ForegroundColorSpan2, NullObject } },
               { TestString1, TestString1.Length-1, 1, new Object[] { NullObject } },
               { TestString2, 0, 1, new Object[] { ForegroundColorSpan2 } },
               { TestString2, 0, TestString2.Length, new Object[] { NullObject, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString2, 2, 1, new Object[] { ForegroundColorSpan1, ForegroundColorSpan2 } },
               { TestString2, 2, TestString2.Length-2, new Object[] { ForegroundColorSpan1, NullObject, ForegroundColorSpan1 } },
               { TestString2, TestString2.Length-1, 1, new Object[] { ForegroundColorSpan1, ForegroundColorSpan3 } },
               { TestStringWithSingleChar, 0, 1, new Object[] { ForegroundColorSpan3, ForegroundColorSpan2, ForegroundColorSpan3, NullObject } },
            };

        public static TheoryData<string, int, int, Object[]> FormatSpannableWithCorrectStartingIndexAndLengthWithDifferentSpansTestData
            => new TheoryData<string, int, int, Object[]>
            {
               { TestString1, 0, 1, new Object[] { NullObject } },
               { TestString1, 0, TestString1.Length, new Object[] { StyleSpan1 } },
               { TestString1, 2, 1, new Object[] { StyleSpan1, ForegroundColorSpan1, StyleSpan3 } },
               { TestString1, 2, TestString1.Length-2, new Object[] { ForegroundColorSpan2, StyleSpan3 } },
               { TestString1, TestString1.Length-1, 1, new Object[] { ForegroundColorSpan1, NullObject } },
               { TestString2, 0, 1, new Object[] { StyleSpan2, ForegroundColorSpan1, StyleSpan2, ForegroundColorSpan2 } },
               { TestString2, 0, TestString2.Length, new Object[] { StyleSpan1, StyleSpan1, NullObject, StyleSpan3 } },
               { TestString2, 2, 1, new Object[] { StyleSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString2, 2, TestString2.Length-2, new Object[] { StyleSpan1, ForegroundColorSpan3, StyleSpan1 } },
               { TestString2, TestString2.Length-1, 1, new Object[] { ForegroundColorSpan3, NullObject, ForegroundColorSpan2 } },
               { TestStringWithSingleChar, 0, 1, new Object[] { ForegroundColorSpan1, StyleSpan2, ForegroundColorSpan1, NullObject } },
            };

        public static TheoryData<string, Object[]> FormatSpannableWithoutStartingIndexAndLengthWithStyleSpansTestData
            => new TheoryData<string, Object[]>
            {
               { TestString1, new Object[] { NullObject } },
               { TestString1, new Object[] { StyleSpan1 } },
               { TestString1, new Object[] { StyleSpan1, NullObject, StyleSpan3 } },
               { TestString1, new Object[] { StyleSpan2, StyleSpan3 } },
               { TestString1, new Object[] { StyleSpan2, NullObject } },
               { TestString2, new Object[] { StyleSpan2 } },
               { TestString2, new Object[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { TestString2, new Object[] { StyleSpan1, StyleSpan2 } },
               { TestString2, new Object[] { StyleSpan1, StyleSpan1, StyleSpan1 } },
               { TestString2, new Object[] { StyleSpan1, StyleSpan3 } },
               { TestStringWithSingleChar, new Object[] { StyleSpan3, StyleSpan2, StyleSpan3, NullObject } },
            };

        public static TheoryData<string, Object[]> FormatSpannableWithoutStartingIndexAndLengthWithForegroundColorSpansTestData
            => new TheoryData<string, Object[]>
            {
               { TestString1, new Object[] { NullObject } },
               { TestString1, new Object[] { ForegroundColorSpan1 } },
               { TestString1, new Object[] { ForegroundColorSpan1, NullObject, ForegroundColorSpan3 } },
               { TestString1, new Object[] { ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString1, new Object[] { ForegroundColorSpan1, NullObject } },
               { TestString2, new Object[] { ForegroundColorSpan2 } },
               { TestString2, new Object[] { ForegroundColorSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString2, new Object[] { ForegroundColorSpan1, ForegroundColorSpan2 } },
               { TestString2, new Object[] { ForegroundColorSpan1, ForegroundColorSpan1, ForegroundColorSpan1 } },
               { TestString2, new Object[] { ForegroundColorSpan1, ForegroundColorSpan3 } },
               { TestStringWithSingleChar, new Object[] { ForegroundColorSpan3, ForegroundColorSpan2, ForegroundColorSpan3, NullObject } },
            };

        public static TheoryData<string, Object[]> FormatSpannableWithoutStartingIndexAndLengthWithDifferentSpansTestData
            => new TheoryData<string, Object[]>
            {
               { TestString1, new Object[] { NullObject } },
               { TestString1, new Object[] { StyleSpan1 } },
               { TestString1, new Object[] { StyleSpan1, ForegroundColorSpan1, StyleSpan3 } },
               { TestString1, new Object[] { ForegroundColorSpan2, NullObject } },
               { TestString1, new Object[] { StyleSpan2, ForegroundColorSpan1, NullObject} },
               { TestString2, new Object[] { StyleSpan2, ForegroundColorSpan1, StyleSpan2, ForegroundColorSpan2 } },
               { TestString2, new Object[] { StyleSpan1, StyleSpan1, NullObject, StyleSpan3 } },
               { TestString2, new Object[] { StyleSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString2, new Object[] { StyleSpan1, NullObject, StyleSpan1 } },
               { TestString2, new Object[] { ForegroundColorSpan3, ForegroundColorSpan3, ForegroundColorSpan2 } },
               { TestStringWithSingleChar, new Object[] { ForegroundColorSpan3, StyleSpan2, ForegroundColorSpan3, NullObject } },
            };
    }
}