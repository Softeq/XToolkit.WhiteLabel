// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Playground.iOS
{
    public static class PlaygroundStyles
    {
        public static UIColor DefaultBackgroundColor =>
            UIDevice.CurrentDevice.CheckSystemVersion(13, 0)
                ? UIColor.SystemBackground
                : UIColor.White;
    }
}
