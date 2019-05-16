// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Softeq.XToolkit.Common.Models;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class NsRangeExtension
    {
        public static TextRange ToTextRange(this NSRange range)
        {
            return new TextRange((int) range.Location, (int) range.Length);
        }

        public static NSRange ToNSRange(this TextRange range)
        {
            return new NSRange(range.Position, range.Length);
        }
    }
}