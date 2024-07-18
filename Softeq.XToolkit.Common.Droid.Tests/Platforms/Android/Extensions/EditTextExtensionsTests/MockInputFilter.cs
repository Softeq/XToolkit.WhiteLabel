// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Text;
using Java.Lang;
using JObject = Java.Lang.Object;

namespace Softeq.XToolkit.Common.Droid.Tests.Extensions.EditTextExtensionsTests;

public class MockInputFilter : JObject, IInputFilter
{
    public ICharSequence? FilterFormatted(ICharSequence? source, int start, int end, ISpanned? dest, int dstart, int dend)
    {
        return null;
    }
}
