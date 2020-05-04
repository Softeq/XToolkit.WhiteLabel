﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Content;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using AndroidX.Core.Content;

namespace Softeq.XToolkit.WhiteLabel.Droid.Controls
{
    /// <summary>
    ///     A child of an abstract ClickableSpan that allows to make a part of text clickable
    ///     with a specified action and removes standard underline decoration.
    /// </summary>
    public class ColoredClickableSpan : ClickableSpan
    {
        private readonly Context _context;
        private readonly Action _clickAction;
        private readonly int _colorResourceId;

        public ColoredClickableSpan(Context context, Action clickAction, int colorResourceId)
        {
            _context = context;
            _clickAction = clickAction;
            _colorResourceId = colorResourceId;
        }

        public override void OnClick(View widget)
        {
            _clickAction?.Invoke();
        }

        public override void UpdateDrawState(TextPaint ds)
        {
            base.UpdateDrawState(ds);
            ds.UnderlineText = false;
            ds.BgColor = ContextCompat.GetColor(_context, _colorResourceId);
        }
    }
}
