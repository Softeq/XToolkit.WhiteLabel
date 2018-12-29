// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public interface IViewLocator
    {
        UIViewController GetView(object model);

        UIViewController GetTopViewController();

        void Initialize(Dictionary<Type,Type> viewmodelToView);
    }
}