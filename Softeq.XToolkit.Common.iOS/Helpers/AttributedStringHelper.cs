// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Extensions;
using System.Text.RegularExpressions;
using Foundation;
using UIKit;
using System;

namespace Softeq.XToolkit.Common.iOS.Helpers
{
    public static class AttributedStringHelper
    {
        public static NSUrl ToNSUrl(this string link)
        {
            var uri = new Uri(link);
            var url = NSUrl.FromString(uri.AbsoluteUri);
            return url;
        }

        public static NSMutableAttributedString BuildAttributedString(this string inputString)
        {
            return new NSMutableAttributedString(inputString);
        }

        public static NSMutableAttributedString BuildAttributedStringFromHtml(this string inputString)
        {
            var importParams = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,

            };

            NSError error = new NSError();

            var attributedString = new NSAttributedString(inputString, importParams, ref error);
            return new NSMutableAttributedString(attributedString);
        }

        public static NSMutableAttributedString Font(this NSMutableAttributedString self, UIFont font)
        {
            self.AddAttribute(UIStringAttributeKey.Font, font, new NSRange(0, self.Length));
            return self;
        }

        public static NSMutableAttributedString Underline(this NSMutableAttributedString self, NSUnderlineStyle underlineStyle = NSUnderlineStyle.Single)
        {
            self.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)underlineStyle), new NSRange(0, self.Length));
            return self;
        }

        public static NSMutableAttributedString Foreground(this NSMutableAttributedString self, UIColor color,
            NSRange? range = null)
        {
            self.AddAttribute(UIStringAttributeKey.ForegroundColor, color, range ?? new NSRange(0, self.Length));
            return self;
        }

        public static NSMutableAttributedString HighlightStrings(
            this NSMutableAttributedString self, IEnumerable<NSRange> ranges, UIColor color)
        {
            foreach (var r in ranges)
            {
                self.Foreground(color, r);
            }

            return self;
        }

        public static NSMutableAttributedString DetectLinks(this NSMutableAttributedString self,
            UIColor color,
            NSUnderlineStyle style,
            bool highlightLink,
            out string[] result)
        {
            var linkNames = new List<string>();
            var i = 0;

            foreach (var link in self.Value.FindLinks())
            {
                var range = new NSRange(link.Index, link.Length);
                var linkName = $"link{i++}";
                var url = link.Value.ToNSUrl();

                if (url == null)
                {
                    continue; // skip URLs which we can't open
                }

                linkNames.Add(linkName);

                self.AddLink(url, linkName, color, style, range);

                if (highlightLink)
                {
                    self.Foreground(color, range);
                }
            }

            result = linkNames.ToArray();

            return self;
        }

        public static NSMutableAttributedString AddLink(this NSMutableAttributedString self, NSUrl url,
            string linkName, UIColor color, NSUnderlineStyle style, NSRange range)
        {
            self.AddAttribute(new NSString(linkName), url, range);
            self.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)style), range);
            self.AddAttribute(UIStringAttributeKey.UnderlineColor, color, range);
            return self;
        }
    }
}