// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public interface IViewComponent<T>
    {
        string Key { get; }

        void Attach(T item);
        void Detach(T item = default(T));
    }
}
