using System;
using Softeq.XToolkit.Bindings.Abstract;

namespace Softeq.XToolkit.Bindings.Droid.Bindable
{
    public interface IBindableViewHolder : IBindableOwner
    {
        event EventHandler ItemClicked;

        void OnAttachedToWindow();
        void OnDetachedFromWindow();
        void OnViewRecycled();
    }
}
