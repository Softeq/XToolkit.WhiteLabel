// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿namespace Softeq.XToolkit.Bindings.Abstract
{
    public interface IBindableOwner : IBindable
    {
        object DataContext { get; set; } // TODO YP: public set only for support current projects

        void SetBindings();
    }
}
