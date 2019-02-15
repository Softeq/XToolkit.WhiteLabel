// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
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