// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Foundation;
using Softeq.XToolkit.Common.Extensions;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class NsStringExtensions
    {
        /// <summary>
        ///     Gets new instance of the default paragraph style.
        /// </summary>
        public static NSMutableParagraphStyle NewParagraphStyle =>
            (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();

        /// <summary>
        ///     Convert string to <see cref="T:Foundation.NSUrl"/> instance.
        /// </summary>
        /// <param name="link">Link.</param>
        /// <returns>NSUrl instance.</returns>
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

        public static NSMutableAttributedString BuildAttributedStringFromHtml(
            this string inputString,
            NSStringEncoding encoding = NSStringEncoding.UTF8)
        {
            var importParams = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,
                StringEncoding = encoding
            };

            var error = new NSError();

            var attributedString = new NSAttributedString(inputString, importParams, ref error);
            return new NSMutableAttributedString(attributedString);
        }

        public static NSMutableAttributedString Font(this NSMutableAttributedString self, UIFont font, NSRange? range = null)
        {
            self.AddAttribute(UIStringAttributeKey.Font, font, range ?? new NSRange(0, self.Length));
            return self;
        }

        public static NSMutableAttributedString Underline(
            this NSMutableAttributedString self,
            NSUnderlineStyle underlineStyle = NSUnderlineStyle.Single,
            NSRange? range = null)
        {
            self.AddAttribute(
                UIStringAttributeKey.UnderlineStyle,
                NSNumber.FromInt32((int) underlineStyle),
                range ?? new NSRange(0, self.Length));
            return self;
        }

        public static NSMutableAttributedString Foreground(
            this NSMutableAttributedString self,
            UIColor color,
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

        /// <summary>
        ///     Set paragraph style of attributed string.
        /// </summary>
        /// <param name="self">Attributed string.</param>
        /// <param name="style">Paragraph style. Use <see cref="NewParagraphStyle" /> for create custom style.</param>
        /// <returns>An instance of a mutable string.</returns>
        public static NSMutableAttributedString ParagraphStyle(
            this NSMutableAttributedString self,
            NSMutableParagraphStyle style)
        {
            self.AddAttribute(UIStringAttributeKey.ParagraphStyle, style, new NSRange(0, self.Length));
            return self;
        }

        public static NSMutableAttributedString DetectLinks(
            this NSMutableAttributedString self,
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

        public static NSMutableAttributedString AddLink(
            this NSMutableAttributedString self,
            NSUrl url,
            string linkName,
            UIColor color,
            NSUnderlineStyle style,
            NSRange range)
        {
            self.AddAttribute(new NSString(linkName), url, range);
            self.AddAttribute(UIStringAttributeKey.UnderlineColor, color, range);
            self.Underline(style, range);
            return self;
        }
    }
}
