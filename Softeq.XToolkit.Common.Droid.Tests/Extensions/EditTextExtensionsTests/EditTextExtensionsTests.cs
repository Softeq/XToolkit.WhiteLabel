// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Text;
using Android.Widget;
using Softeq.XToolkit.Common.Droid.Extensions;
using Softeq.XToolkit.Common.Droid.TextFilters;
using Xunit;

namespace Softeq.XToolkit.Common.Droid.Tests.Extensions.EditTextExtensionsTests
{
    public class EditTextExtensionsTests
    {
        [Fact]
        public void SetFilters_WhenCalledWithoutFilters_DoNothing()
        {
            EditText editText = new EditText(MainActivity.Current);
            editText.SetFilters();

            var appliedFilters = editText.GetFilters();
            if (appliedFilters != null)
            {
                Assert.Empty(appliedFilters);
            }
        }

        [Fact]
        public void SetFilters_WhenCalledWithMultipleFilters_AppliesAllFilters()
        {
            EditText editText = new EditText(MainActivity.Current);

            var filter1 = new ForbiddenCharsInputFilter(new char[] { 'a' });
            var filter2 = new ForbiddenCharsInputFilter(new char[] { 'b' });
            var filter3 = new ForbiddenCharsInputFilter(new char[] { 'a' });
            var filters = new IInputFilter[] { filter1, filter2, filter3 };

            editText.SetFilters(filter1, filter2, filter3);

            var appliedFilters = editText.GetFilters();
            Assert.Equal(filters, appliedFilters);
        }

        [Fact]
        public void SetFilters_WhenCalledWithMultipleFiltersWithNullAndDuplicates_AppliesAllFilters()
        {
            EditText editText = new EditText(MainActivity.Current);

            var filter1 = new ForbiddenCharsInputFilter(new char[] { 'a' });
            var filter2 = new ForbiddenCharsInputFilter(new char[] { 'b' });
            var filter3 = new ForbiddenCharsInputFilter(new char[] { 'a' });
            var nullFilter = null as IInputFilter;
            var filters = new IInputFilter[] { filter1, filter2, nullFilter, filter3, filter2, nullFilter, filter1, filter2 };

            editText.SetFilters(filter1, filter2, nullFilter, filter3, filter2, nullFilter, filter1, filter2);

            var appliedFilters = editText.GetFilters();
            Assert.Equal(filters, appliedFilters);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("abcd")]
        [InlineData("abcdefg")]
        public void KeepFocusAtTheEndOfField_WhenTextChangedWithoutFocus_WhenFocused_MovesCursorToTheEndOfField(string str)
        {
            EditText editText = new EditText(MainActivity.Current);
            editText.ClearFocus();

            editText.Text = str;
            editText.RequestFocus();

            Assert.Equal(editText.SelectionEnd, editText.SelectionStart);
            Assert.Equal(editText.SelectionStart, str.Length);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("abcd")]
        [InlineData("abcdefg")]
        public void KeepFocusAtTheEndOfField_WhenTextChangedWithFocus_WhenRefocused_MovesCursorToTheEndOfField(string str)
        {
            EditText editText = new EditText(MainActivity.Current);
            editText.RequestFocus();

            editText.Text = str;
            editText.ClearFocus();
            editText.RequestFocus();

            Assert.Equal(editText.SelectionEnd, editText.SelectionStart);
            Assert.Equal(editText.SelectionStart, str.Length);
        }
    }
}
