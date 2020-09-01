// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Drawing;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Softeq.XToolkit.Common.Droid.Extensions;
using Softeq.XToolkit.WhiteLabel.Helpers;
using Color = Android.Graphics.Color;
using PointF = Android.Graphics.PointF;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    public class AvatarPlaceholderDrawable : Drawable
    {
        private const float TextSizePercentage = 40;
        private readonly string _avatarText;
        private readonly Paint _backgroundPaint;
        private readonly int _size;

        private readonly Paint _textPaint;
        private readonly Context _context;
        private RectF? _placeholderBounds;
        private float _textStartXPoint;
        private float _textStartYPoint;

        public AvatarPlaceholderDrawable(Context context, string name, AvatarStyles styles)
        {
            _context = context;

            var info = AvatarPlaceholderBuilder.Build(name, styles.BackgroundHexColors);

            _avatarText = info.Text;

            _textPaint = new Paint
            {
                AntiAlias = true,
                Color = Color.White
            };
            _textPaint.SetTypeface(Typeface.Create("sans-serif", TypefaceStyle.Bold));

            _backgroundPaint = new Paint
            {
                AntiAlias = true
            };
            _backgroundPaint.SetStyle(Paint.Style.Fill);
            _backgroundPaint.Color = Color.ParseColor(info.Color);
            _size = Math.Min(styles.Size.Width, styles.Size.Height);
        }

        public override int Opacity => 1;

        public override void Draw(Canvas canvas)
        {
            if (_placeholderBounds == null)
            {
                var centerPoint = new PointF(canvas.Width / 2f, canvas.Height / 2f);
                var width = _context.ToPixels(_size);
                var sizeInPixels = new SizeF(width, width);
                var x = centerPoint.X - (sizeInPixels.Width / 2f);
                var y = centerPoint.Y - (sizeInPixels.Height / 2f);

                _placeholderBounds = new RectF(
                    x,
                    y,
                    x + sizeInPixels.Width,
                    y + sizeInPixels.Height);

                SetAvatarTextValues();
            }

            canvas.DrawCircle(
                canvas.Width / 2f,
                canvas.Height / 2f,
                _placeholderBounds.Width() / 2f,
                _backgroundPaint);

            canvas.DrawText(_avatarText, _textStartXPoint, _textStartYPoint, _textPaint);
        }

        public override void SetAlpha(int alpha)
        {
            _textPaint.Alpha = alpha;
            _backgroundPaint.Alpha = alpha;
        }

        public override void SetColorFilter(ColorFilter colorFilter)
        {
            _textPaint.SetColorFilter(colorFilter);
            _backgroundPaint.SetColorFilter(colorFilter);
        }

        private void SetAvatarTextValues()
        {
            _textPaint.TextSize = CalculateTextSize();
            _textStartXPoint = CalculateTextStartXPoint();
            _textStartYPoint = CalculateTextStartYPoint();
        }

        private float CalculateTextStartXPoint()
        {
            var stringWidth = _textPaint.MeasureText(_avatarText);
            return (Bounds.Width() / 2f) - (stringWidth / 2f);
        }

        private float CalculateTextStartYPoint()
        {
            return (Bounds.Height() / 2f) - ((_textPaint.Ascent() + _textPaint.Descent()) / 2f);
        }

        private float CalculateTextSize()
        {
            return _placeholderBounds!.Height() * TextSizePercentage / 100;
        }

        public class AvatarStyles
        {
            public AvatarStyles(Size size, string[] backgroundHexColors)
            {
                Size = size;
                BackgroundHexColors = backgroundHexColors;
            }

            public Size Size { get; set; }
            public string[] BackgroundHexColors { get; set; }
        }
    }
}
