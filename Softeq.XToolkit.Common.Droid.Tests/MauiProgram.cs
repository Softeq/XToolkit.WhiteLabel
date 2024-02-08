// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using Xunit.Runners.Maui;

namespace Softeq.XToolkit.Common.Droid.Tests;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp
            .CreateBuilder()
            .ConfigureTests(new TestOptions
            {
                Assemblies =
                {
                    typeof(MauiProgram).Assembly
                }
            })
            .UseVisualRunner();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
