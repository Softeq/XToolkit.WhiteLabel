// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Foundation;
using Softeq.XToolkit.Common.Extensions;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class NSStringExtensions
    {
        /// <summary>
        ///     Gets new instance of the default paragraph style.
        /// </summary>
        public static NSMutableParagraphStyle NewParagraphStyle =>
            (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();

        /// <summary>
        ///     Convert string to <see cref="T:Foundation.NSUrl"/> instance.
        /// </summary>
        /// <remarks>
        ///     If the pattern matches example.com, which does not have a URL scheme prefix,
        ///     the supplied scheme will be prepended to create http://example.com when the clickable URL link is created.
        /// </remarks>
        /// <param name="link">Link.</param>
        /// <returns><see cref="T:Foundation.NSUrl"/> instance.</returns>
        public static NSUrl ToNSUrl(this string link)
        {
            var uri = new UriBuilder(link).Uri;
            var url = NSUrl.FromString(uri.AbsoluteUri);
            return url;
        }

        /// <summary>
        ///     Creates <see cref="T:Foundation.NSMutableAttributedString"/> instance from <paramref name="input"/> string.
        /// </summary>
        /// <param name="input">Any string.</param>
        /// <returns>New instance of <see cref="T:Foundation.NSMutableAttributedString"/>.</returns>
        public static NSMutableAttributedString BuildAttributedString(this string input)
        {
            return new NSMutableAttributedString(input);
        }

        /// <summary>
        ///     Creates <see cref="T:Foundation.NSMutableAttributedString"/> instance from <paramref name="html"/> string.
        /// </summary>
        /// <param name="html">Any HTML string.</param>
        /// <param name="encoding">HTML encoding.</param>
        /// <returns>New instance of <see cref="T:Foundation.NSMutableAttributedString"/>.</returns>
        public static NSMutableAttributedString BuildAttributedStringFromHtml(
            this string html,
            NSStringEncoding encoding = NSStringEncoding.UTF8)
        {
            var importParams = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,
                StringEncoding = encoding
            };

            var error = new NSError();

            var attributedString = new NSAttributedString(html, importParams, ref error);
            return new NSMutableAttributedString(attributedString);
        }

        /// <summary>
        ///     Set font for range.
        /// </summary>
        /// <param name="self">Target.</param>
        /// <param name="font">Font.</param>
        /// <param name="range">Range to set font.</param>
        /// <returns>Modified instance of <see cref="T:Foundation.NSMutableAttributedString"/>.</returns>
        public static NSMutableAttributedString Font(
            this NSMutableAttributedString self, UIFont font, NSRange? range = null)
        {
            self.AddAttribute(UIStringAttributeKey.Font, font, range ?? new NSRange(0, self.Length));
            return self;
        }

        /// <summary>
        ///    Set underline for range.
        /// </summary>
        /// <param name="self">Target.</param>
        /// <param name="underlineStyle">Style.</param>
        /// <param name="range">Range to set underline.</param>
        /// <returns>Modified instance of <see cref="T:Foundation.NSMutableAttributedString"/>.</returns>
        public static NSMutableAttributedString Underline(
            this NSMutableAttributedString self,
            NSUnderlineStyle underlineStyle = NSUnderlineStyle.Single,
            NSRange? range = null)
        {
            var value = NSNumber.FromInt32((int) underlineStyle);
            self.AddAttribute(UIStringAttributeKey.UnderlineStyle, value, range ?? new NSRange(0, self.Length));
            return self;
        }

        /// <summary>
        ///    Set foreground color for range.
        /// </summary>
        /// <param name="self">Target.</param>
        /// <param name="color">Color.</param>
        /// <param name="range">Range to set color.</param>
        /// <returns>Modified instance of <see cref="T:Foundation.NSMutableAttributedString"/>.</returns>
        public static NSMutableAttributedString Foreground(
            this NSMutableAttributedString self,
            UIColor color,
            NSRange? range = null)
        {
            self.AddAttribute(UIStringAttributeKey.ForegroundColor, color, range ?? new NSRange(0, self.Length));
            return self;
        }

        /// <summary>
        ///    Set foreground color for the ranges.
        /// </summary>
        /// <param name="self">Target.</param>
        /// <param name="ranges">Ranges to set color.</param>
        /// <param name="color">Range to set color.</param>
        /// <returns>Modified instance of <see cref="T:Foundation.NSMutableAttributedString"/>.</returns>
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
        ///     Set paragraph style.
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

        /// <summary>
        ///    Auto-detect links.
        /// </summary>
        /// <remarks>
        ///     If you are matching web URLs you would supply the scheme http://.
        ///     If the pattern matches example.com, which does not have a URL scheme prefix,
        ///     the supplied scheme will be prepended to create http://example.com when the clickable URL link is created.
        /// </remarks>
        /// <param name="self">Target.</param>
        /// <param name="color">Link color.</param>
        /// <param name="style">Link style.</param>
        /// <param name="highlightLink">Flag to highlight link.</param>
        /// <param name="resultLinkNames">List of link names.</param>
        /// <returns>Modified instance of <see cref="T:Foundation.NSMutableAttributedString"/>.</returns>
        public static NSMutableAttributedString DetectLinks(
            this NSMutableAttributedString self,
            UIColor color,
            NSUnderlineStyle style,
            bool highlightLink,
            out string[] resultLinkNames)
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

            resultLinkNames = linkNames.ToArray();

            return self;
        }

        /// <summary>
        ///    Set link for range.
        /// </summary>
        /// <param name="self">Target.</param>
        /// <param name="url">URL link.</param>
        /// <param name="linkName">Link name.</param>
        /// <param name="color">Link color.</param>
        /// <param name="style">Link style.</param>
        /// <param name="range">Range to set link.</param>
        /// <returns>Modified instance of <see cref="T:Foundation.NSMutableAttributedString"/>.</returns>
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
