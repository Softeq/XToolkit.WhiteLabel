// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Drawing;
using Android.Graphics;
using Android.Graphics.Drawables;
using Softeq.XToolkit.WhiteLabel.Helpers;
using Color = Android.Graphics.Color;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    public class AvatarPlaceholderDrawable : Drawable
    {
        private const float TextSizePercentage = 40;

        private readonly Paint _textPaint;
        private readonly Paint _backgroundPaint;
        private readonly string _avatarText;
        
        private RectF _placeholderBounds;
        private float _textStartXPoint;
        private float _textStartYPoint;

        public override int Opacity => 1;

        public AvatarPlaceholderDrawable(string name, AvatarStyles styles)
        {
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
        }

        public override void Draw(Canvas canvas)
        {
            if (_placeholderBounds == null)
            {
                _placeholderBounds = new RectF(0, 0, canvas.Width, canvas.Height);
                SetAvatarTextValues();
            }

            canvas.DrawCircle(
                _placeholderBounds.CenterX(),
                _placeholderBounds.CenterY(),
                _placeholderBounds.Width() / 2,
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
            float stringWidth = _textPaint.MeasureText(_avatarText);
            return (Bounds.Width() / 2f) - (stringWidth / 2f);
        }

        private float CalculateTextStartYPoint()
        {
            return (Bounds.Height() / 2f) - ((_textPaint.Ascent() + _textPaint.Descent()) / 2f);
        }

        private float CalculateTextSize()
        {
            return Bounds.Height() * TextSizePercentage / 100;
        }

        public class AvatarStyles
        {
            public Size Size { get; set; }
            public string[] BackgroundHexColors { get; set; }
        }
    }
}
