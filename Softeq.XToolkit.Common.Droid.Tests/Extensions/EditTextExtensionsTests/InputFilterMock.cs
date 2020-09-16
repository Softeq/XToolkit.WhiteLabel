// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Text;
using Java.Lang;

namespace Softeq.XToolkit.Common.Droid.Tests.Extensions.EditTextExtensionsTests
{
    public class InputFilterMock : Object, IInputFilter
    {
        public ICharSequence? FilterFormatted(ICharSequence? source, int start, int end, ISpanned? dest, int dstart, int dend)
        {
            return null;
        }
    }
}
