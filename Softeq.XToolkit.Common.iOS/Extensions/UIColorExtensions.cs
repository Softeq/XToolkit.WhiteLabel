// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class UIColorExtensions
    {
        public static UIColor UIColorFromHex(this string hexValue, float alpha = 1.0f)
        {
            var hexString = hexValue.Replace("#", string.Empty);

            alpha = Math.Clamp(alpha, 0.0f, 1.0f);

            float red, green, blue;

            switch (hexString.Length)
            {
                case 3: // #RGB
                {
                    red = Convert.ToInt32(string.Format("{0}{0}", hexString.Substring(0, 1)), 16) / 255f;
                    green = Convert.ToInt32(string.Format("{0}{0}", hexString.Substring(1, 1)), 16) / 255f;
                    blue = Convert.ToInt32(string.Format("{0}{0}", hexString.Substring(2, 1)), 16) / 255f;
                    return UIColor.FromRGBA(red, green, blue, alpha);
                }

                case 6: // #RRGGBB
                {
                    red = Convert.ToInt32(hexString.Substring(0, 2), 16) / 255f;
                    green = Convert.ToInt32(hexString.Substring(2, 2), 16) / 255f;
                    blue = Convert.ToInt32(hexString.Substring(4, 2), 16) / 255f;
                    return UIColor.FromRGBA(red, green, blue, alpha);
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        $"Invalid color value {hexValue} is invalid. It should be a hex value of the form #RBG, #RRGGBB");
            }
        }

        public static string ToHex(this UIColor color)
        {
            color.GetRGBA(out var r, out var g, out var b, out _);

            return $"#{ToHexPart(r)}{ToHexPart(g)}{ToHexPart(b)}";
        }

        private static string ToHexPart(nfloat value)
        {
            return ((int)(value * 255)).ToString("X2");
        }
    }
}
