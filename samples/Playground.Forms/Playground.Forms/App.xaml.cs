// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Playground.Forms.Views;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Xamarin.Forms;

namespace Playground.Forms
{
    public partial class App
    {
        public App(IBootstrapper bootstrapper, Func<List<Assembly>> getAssembliesFunc)
            : base(bootstrapper, getAssembliesFunc)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new StartPage());
        }
    }
}
