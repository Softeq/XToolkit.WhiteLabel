// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Services;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Playground
{
    public static class CustomBootstrapper
    {
        public static void Configure(IContainerBuilder builder)
        {
            // common
            builder.Singleton<JsonSerializer, IJsonSerializer>();

            // playgroud
            builder.Singleton<DataService, IDataService>();
        }
    }
}
