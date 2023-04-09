// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;
using Android.Text.Style;
using Softeq.XToolkit.Common.Helpers;
using Xunit;
using AndroidGraphics = Android.Graphics;
using JObject = Java.Lang.Object;
using JString = Java.Lang.String;

namespace Softeq.XToolkit.Common.Droid.Tests.Extensions.StringExtensionsTests
{
    [SuppressMessage("ReSharper", "RedundantExplicitArrayCreation", Justification = "Just for tests.")]
    internal static class StringExtensionsDataProvider
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public static readonly TextRange NullTextRange = null;
        private static readonly JObject NullObject = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        private static readonly string TestString1 = "abcd";
        private static readonly string TestString2 = "abcdefghijk";
        private static readonly string TestStringWithSingleChar = "a";

        private static readonly StyleSpan StyleSpan1 = new StyleSpan(AndroidGraphics.TypefaceStyle.Bold);
        private static readonly StyleSpan StyleSpan2 = new StyleSpan(AndroidGraphics.TypefaceStyle.Italic);
        private static readonly StyleSpan StyleSpan3 = new StyleSpan(AndroidGraphics.TypefaceStyle.Normal);

        private static readonly ForegroundColorSpan ForegroundColorSpan1 = new ForegroundColorSpan(AndroidGraphics.Color.White);
        private static readonly ForegroundColorSpan ForegroundColorSpan2 = new ForegroundColorSpan(AndroidGraphics.Color.Transparent);
        private static readonly ForegroundColorSpan ForegroundColorSpan3 = new ForegroundColorSpan(AndroidGraphics.Color.Red);

        public static TheoryData<string> FormatSpannableStringsTestData
           => new TheoryData<string>
           {
               { string.Empty },
               { TestString1 },
               { TestString2 },
               { TestStringWithSingleChar },
           };

        public static TheoryData<TextRange, JObject[]> FormatSpannableNullStringWithRangeAndSpansTestData
           => new TheoryData<TextRange, JObject[]>
           {
               { new TextRange(0, 0), new JObject[] { } },
               { new TextRange(0, 0), new JObject[] { StyleSpan1 } },
               { new TextRange(0, 0), new JObject[] { StyleSpan1, ForegroundColorSpan1, StyleSpan2 } },
               { new TextRange(1, 5), new JObject[] { } },
               { new TextRange(1, 5), new JObject[] { ForegroundColorSpan2 } },
               { new TextRange(1, 5), new JObject[] { ForegroundColorSpan2, StyleSpan2, ForegroundColorSpan3 } },
               { new TextRange(5, 1), new JObject[] { NullObject } },
               { new TextRange(5, 1), new JObject[] { new JString() } },
               { new TextRange(5, 1), new JObject[] { StyleSpan1, new JString(), NullObject } },
               { new TextRange(2, 0), new JObject[] { } },
               { new TextRange(2, 0), new JObject[] { ForegroundColorSpan3 } },
               { new TextRange(2, 0), new JObject[] { StyleSpan3, NullObject, StyleSpan2 } },
               { new TextRange(0, 2), new JObject[] { } },
               { new TextRange(0, 2), new JObject[] { ForegroundColorSpan2 } },
               { new TextRange(0, 2), new JObject[] { StyleSpan3, NullObject, StyleSpan1 } },
               { NullTextRange, new JObject[] { NullObject } },
               { NullTextRange, new JObject[] { StyleSpan3 } },
               { NullTextRange, new JObject[] { ForegroundColorSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
           };

        public static TheoryData<TextRange> FormatSpannableNullStringWithoutSpansTestData
           => new TheoryData<TextRange>
           {
               { new TextRange(0, 0) },
               { new TextRange(0, 2) },
               { new TextRange(2, 0) },
               { new TextRange(1, 5) },
               { new TextRange(5, 1) },
               { NullTextRange }
           };

        public static TheoryData<JObject[]> FormatSpannableNullStringWithSpansOnlyTestData
           => new TheoryData<JObject[]>
           {
               { new JObject[] { } },
               { new JObject[] { NullObject } },
               { new JObject[] { StyleSpan1 } },
               { new JObject[] { StyleSpan1, NullObject } },
               { new JObject[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { new JObject[] { StyleSpan1, ForegroundColorSpan1, ForegroundColorSpan1 } },
               { new JObject[] { StyleSpan1, new JString() } },
               { new JObject[] { StyleSpan1, NullObject, new JString(), ForegroundColorSpan1 } },
               { new JObject[] { StyleSpan1, NullObject, new JString(), StyleSpan1 } },
           };

        public static TheoryData<string, TextRange> FormatSpannableWithNonNullTextRangeWithoutSpansTestData
            => new TheoryData<string, TextRange>
            {
               // incorrect
               { TestString1, new TextRange(0, 0) },
               { TestString1, new TextRange(0, 1) },
               { TestString1, new TextRange(0, TestString1.Length + 1) },
               { TestString1, new TextRange(2, 0) },
               { TestString1, new TextRange(2, TestString1.Length-1) },
               { TestString1, new TextRange(2, TestString1.Length) },
               { TestString1, new TextRange(2, TestString1.Length + 1) },
               { TestString1, new TextRange(TestString1.Length, 0) },
               { TestString1, new TextRange(TestString1.Length, TestString1.Length-1) },
               { TestString1, new TextRange(TestString1.Length, TestString1.Length) },
               { TestString1, new TextRange(TestString1.Length, TestString1.Length + 1) },
               { TestString1, new TextRange(TestString1.Length + 1, 0) },
               { TestString1, new TextRange(TestString1.Length + 1, TestString1.Length-1) },
               { TestString1, new TextRange(TestString1.Length + 1, TestString1.Length) },
               { TestString1, new TextRange(TestString1.Length + 1, TestString1.Length + 1) },

               // correct
               { TestString1, new TextRange(0, 1) },
               { TestString1, new TextRange(0, TestString1.Length) },
               { TestString1, new TextRange(2, 1) },
               { TestString1, new TextRange(2, TestString1.Length-2) },
               { TestString1, new TextRange(TestString1.Length-1, 1) },
               { TestString2, new TextRange(0, 1) },
               { TestString2, new TextRange(0, TestString2.Length) },
               { TestString2, new TextRange(2, 1) },
               { TestString2, new TextRange(2, TestString2.Length-2) },
               { TestString2, new TextRange(TestString2.Length-1, 1) },
               { TestStringWithSingleChar, new TextRange(0, 1) },
            };

        public static TheoryData<string, TextRange, JObject[]> FormatSpannableWithIncorrectTextRangeWithSpansTestData
            => new TheoryData<string, TextRange, JObject[]>
            {
               { TestString1, new TextRange(0, TestString1.Length + 1), new JObject[] { StyleSpan1, StyleSpan2 } },
               { TestString1, new TextRange(2, TestString1.Length-1), new JObject[] { ForegroundColorSpan2 } },
               { TestString1, new TextRange(2, TestString1.Length), new JObject[] { StyleSpan3 } },
               { TestString1, new TextRange(2, TestString1.Length + 1), new JObject[] { StyleSpan2, StyleSpan3 } },
               { TestString1, new TextRange(1, TestString1.Length), new JObject[] { new JString(), StyleSpan2 } },
               { TestString1, new TextRange(1, TestString1.Length + 1), new JObject[] { StyleSpan1, ForegroundColorSpan1 } },
               { TestString1, new TextRange(TestString1.Length, TestString1.Length-1), new JObject[] { StyleSpan3, NullObject } },
               { TestString1, new TextRange(TestString1.Length, 1), new JObject[] { StyleSpan1, StyleSpan1, StyleSpan3 } },
               { TestString1, new TextRange(TestString1.Length, TestString1.Length), new JObject[] { ForegroundColorSpan1, NullObject } },
               { TestString1, new TextRange(TestString1.Length, TestString1.Length + 1), new JObject[] { new JString(), NullObject } },
               { TestString1, new TextRange(TestString1.Length + 1, TestString1.Length-1), new JObject[] { new JString(), StyleSpan2 } },
               { TestString1, new TextRange(TestString1.Length + 1, TestString1.Length), new JObject[] { StyleSpan1, NullObject, new JString() } },
               { TestString1, new TextRange(TestString1.Length + 1, TestString1.Length + 1), new JObject[] { ForegroundColorSpan2 } },
            };

        public static TheoryData<string, TextRange, JObject[]> FormatSpannableWithCorrectTextRangeWithStyleSpansTestData
            => new TheoryData<string, TextRange, JObject[]>
            {
               { TestString1, new TextRange(0, 0), new JObject[] { StyleSpan2, NullObject, StyleSpan2 } },
               { TestString1, new TextRange(0, 1), new JObject[] { StyleSpan2, NullObject } },
               { TestString1, new TextRange(0, TestString1.Length), new JObject[] { StyleSpan1 } },
               { TestString1, new TextRange(2, 0), new JObject[] { StyleSpan1, StyleSpan1, StyleSpan1 } },
               { TestString1, new TextRange(2, 1), new JObject[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { TestString1, new TextRange(2, TestString1.Length-2), new JObject[] { StyleSpan2, StyleSpan3 } },
               { TestString1, new TextRange(TestString1.Length-1, 1), new JObject[] { NullObject } },
               { TestString2, new TextRange(0, 1), new JObject[] { StyleSpan2 } },
               { TestString2, new TextRange(0, TestString2.Length), new JObject[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { TestString2, new TextRange(2, 1), new JObject[] { StyleSpan1, StyleSpan2 } },
               { TestString2, new TextRange(2, TestString2.Length-2), new JObject[] { StyleSpan1, StyleSpan1, StyleSpan1 } },
               { TestString2, new TextRange(TestString2.Length-1, 1), new JObject[] { StyleSpan1, NullObject } },
               { TestStringWithSingleChar, new TextRange(0, 0), new JObject[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { TestStringWithSingleChar, new TextRange(0, 1), new JObject[] { StyleSpan3, StyleSpan2, StyleSpan3, NullObject } },
            };

        public static TheoryData<string, TextRange, JObject[]> FormatSpannableWithCorrectTextRangeWithForegroundColorSpansTestData
            => new TheoryData<string, TextRange, JObject[]>
            {
               { TestString1, new TextRange(0, 0), new JObject[] { ForegroundColorSpan1, ForegroundColorSpan2, NullObject } },
               { TestString1, new TextRange(0, 1), new JObject[] { ForegroundColorSpan1 } },
               { TestString1, new TextRange(0, TestString1.Length), new JObject[] { ForegroundColorSpan1 } },
               { TestString1, new TextRange(2, 0), new JObject[] { ForegroundColorSpan2, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString1, new TextRange(2, 1), new JObject[] { ForegroundColorSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString1, new TextRange(2, TestString1.Length-2), new JObject[] { ForegroundColorSpan2, NullObject } },
               { TestString1, new TextRange(TestString1.Length-1, 1), new JObject[] { NullObject } },
               { TestString2, new TextRange(0, 1), new JObject[] { ForegroundColorSpan2 } },
               { TestString2, new TextRange(0, TestString2.Length), new JObject[] { NullObject, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString2, new TextRange(2, 1), new JObject[] { ForegroundColorSpan1, ForegroundColorSpan2 } },
               { TestString2, new TextRange(2, TestString2.Length-2), new JObject[] { ForegroundColorSpan1, NullObject, ForegroundColorSpan1 } },
               { TestString2, new TextRange(TestString2.Length-1, 1), new JObject[] { ForegroundColorSpan1, ForegroundColorSpan3 } },
               { TestStringWithSingleChar, new TextRange(0, 0), new JObject[] { ForegroundColorSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestStringWithSingleChar, new TextRange(0, 1), new JObject[] { ForegroundColorSpan3, ForegroundColorSpan2, ForegroundColorSpan3, NullObject } },
            };

        public static TheoryData<string, TextRange, JObject[]> FormatSpannableWithCorrectTextRangeWithDifferentSpansTestData
            => new TheoryData<string, TextRange, JObject[]>
            {
               { TestString1, new TextRange(0, 0), new JObject[] { NullObject, StyleSpan1, StyleSpan1 } },
               { TestString1, new TextRange(0, 1), new JObject[] { NullObject } },
               { TestString1, new TextRange(0, TestString1.Length), new JObject[] { StyleSpan1 } },
               { TestString1, new TextRange(2, 0), new JObject[] { StyleSpan1, ForegroundColorSpan2, StyleSpan3, NullObject } },
               { TestString1, new TextRange(2, 1), new JObject[] { StyleSpan1, ForegroundColorSpan1, StyleSpan3 } },
               { TestString1, new TextRange(2, TestString1.Length-2), new JObject[] { ForegroundColorSpan2, StyleSpan3 } },
               { TestString1, new TextRange(TestString1.Length-1, 1), new JObject[] { ForegroundColorSpan1, NullObject } },
               { TestString2, new TextRange(0, 1), new JObject[] { StyleSpan2, ForegroundColorSpan1, StyleSpan2, ForegroundColorSpan2 } },
               { TestString2, new TextRange(0, TestString2.Length), new JObject[] { StyleSpan1, StyleSpan1, NullObject, StyleSpan3 } },
               { TestString2, new TextRange(2, 1), new JObject[] { StyleSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString2, new TextRange(2, TestString2.Length-2), new JObject[] { StyleSpan1, ForegroundColorSpan3, StyleSpan1 } },
               { TestString2, new TextRange(TestString2.Length-1, 1), new JObject[] { ForegroundColorSpan3, NullObject, ForegroundColorSpan2 } },
               { TestStringWithSingleChar, new TextRange(0, 0), new JObject[] { ForegroundColorSpan1, StyleSpan2, ForegroundColorSpan2 } },
               { TestStringWithSingleChar, new TextRange(0, 1), new JObject[] { ForegroundColorSpan1, StyleSpan2, ForegroundColorSpan1, NullObject } },
            };

        public static TheoryData<string, JObject[]> FormatSpannableWithoutTextRangeWithStyleSpansTestData
            => new TheoryData<string, JObject[]>
            {
               { TestString1, new JObject[] { NullObject } },
               { TestString1, new JObject[] { StyleSpan1 } },
               { TestString1, new JObject[] { StyleSpan1, NullObject, StyleSpan3 } },
               { TestString1, new JObject[] { StyleSpan2, StyleSpan3 } },
               { TestString1, new JObject[] { StyleSpan2, NullObject } },
               { TestString2, new JObject[] { StyleSpan2 } },
               { TestString2, new JObject[] { StyleSpan1, StyleSpan2, StyleSpan3 } },
               { TestString2, new JObject[] { StyleSpan1, StyleSpan2 } },
               { TestString2, new JObject[] { StyleSpan1, StyleSpan1, StyleSpan1 } },
               { TestString2, new JObject[] { StyleSpan1, StyleSpan3 } },
               { TestStringWithSingleChar, new JObject[] { StyleSpan3, StyleSpan2, StyleSpan3, NullObject } },
            };

        public static TheoryData<string, JObject[]> FormatSpannableWithoutTextRangeWithForegroundColorSpansTestData
            => new TheoryData<string, JObject[]>
            {
               { TestString1, new JObject[] { NullObject } },
               { TestString1, new JObject[] { ForegroundColorSpan1 } },
               { TestString1, new JObject[] { ForegroundColorSpan1, NullObject, ForegroundColorSpan3 } },
               { TestString1, new JObject[] { ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString1, new JObject[] { ForegroundColorSpan1, NullObject } },
               { TestString2, new JObject[] { ForegroundColorSpan2 } },
               { TestString2, new JObject[] { ForegroundColorSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString2, new JObject[] { ForegroundColorSpan1, ForegroundColorSpan2 } },
               { TestString2, new JObject[] { ForegroundColorSpan1, ForegroundColorSpan1, ForegroundColorSpan1 } },
               { TestString2, new JObject[] { ForegroundColorSpan1, ForegroundColorSpan3 } },
               { TestStringWithSingleChar, new JObject[] { ForegroundColorSpan3, ForegroundColorSpan2, ForegroundColorSpan3, NullObject } },
            };

        public static TheoryData<string, JObject[]> FormatSpannableWithoutTextRangeWithDifferentSpansTestData
            => new TheoryData<string, JObject[]>
            {
               { TestString1, new JObject[] { NullObject } },
               { TestString1, new JObject[] { StyleSpan1 } },
               { TestString1, new JObject[] { StyleSpan1, ForegroundColorSpan1, StyleSpan3 } },
               { TestString1, new JObject[] { ForegroundColorSpan2, NullObject } },
               { TestString1, new JObject[] { StyleSpan2, ForegroundColorSpan1, NullObject } },
               { TestString2, new JObject[] { StyleSpan2, ForegroundColorSpan1, StyleSpan2, ForegroundColorSpan2 } },
               { TestString2, new JObject[] { StyleSpan1, StyleSpan1, NullObject, StyleSpan3 } },
               { TestString2, new JObject[] { StyleSpan1, ForegroundColorSpan2, ForegroundColorSpan3 } },
               { TestString2, new JObject[] { StyleSpan1, NullObject, StyleSpan1 } },
               { TestString2, new JObject[] { ForegroundColorSpan3, ForegroundColorSpan3, ForegroundColorSpan2 } },
               { TestStringWithSingleChar, new JObject[] { ForegroundColorSpan3, StyleSpan2, ForegroundColorSpan3, NullObject } },
            };
    }
}
