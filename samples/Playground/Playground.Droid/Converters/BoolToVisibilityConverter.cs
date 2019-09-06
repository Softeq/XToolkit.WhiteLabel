// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Views;
using Softeq.XToolkit.Common.Interfaces;

namespace Playground.Droid.Converters
{
    public class BoolToVisibilityConverter : IConverter<ViewStates, bool>
    {
        public ViewStates ConvertValue(bool value, object parameter = null, string language = null)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }

        public bool ConvertValueBack(ViewStates value, object parameter = null, string language = null)
        {
            throw new NotImplementedException();
        }
    }
}
