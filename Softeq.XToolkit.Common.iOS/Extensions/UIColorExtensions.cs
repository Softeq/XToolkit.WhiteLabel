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
            var colorString = hexValue.StartsWith("#", StringComparison.Ordinal) ? hexValue.Replace("#", "") : hexValue;
            if (alpha > 1.0f)
            {
                alpha = 1.0f;
            }
            else if (alpha < 0.0f)
            {
                alpha = 0.0f;
            }

            float red, green, blue;

            switch (colorString.Length)
            {
                case 3: // #RGB
                {
                    red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                    green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                    blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                    return UIColor.FromRGBA(red, green, blue, alpha);
                }
                case 6: // #RRGGBB
                {
                    red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                    green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                    blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                    return UIColor.FromRGBA(red, green, blue, alpha);
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        string.Format(
                            "Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB",
                            hexValue));
            }
        }

        public static string ToHex(this UIColor color)
        {
            color.GetRGBA(out nfloat r, out nfloat g, out nfloat b, out nfloat a);
            

            return $"#{ToHexPart(r)}{ToHexPart(g)}{ToHexPart(b)}";
        }

        private static string ToHexPart(nfloat value)
        {
            return ((int)(value * 256)).ToString("X2");
        }
    }
}