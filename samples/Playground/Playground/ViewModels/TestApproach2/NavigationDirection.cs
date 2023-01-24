// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;

namespace Playground.ViewModels.TestApproach2
{
    public class NavigationDirection
    {
        public ICommand Next { get; set; }

        public ICommand Back { get; set; }
    }
}
