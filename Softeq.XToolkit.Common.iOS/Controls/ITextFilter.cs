// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Controls
{
    public interface ITextFilter
    {
        bool ShouldChangeText(
            UIResponder responder,
            string? oldText,
            NSRange range,
            string replacementString);
    }
}
