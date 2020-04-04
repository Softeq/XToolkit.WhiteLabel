﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Services;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Playground
{
    public static class CustomBootstrapper
    {
        public static void Configure(IContainerBuilder builder)
        {
            // playgroud
            builder.Singleton<DataService, IDataService>();
        }
    }
}
