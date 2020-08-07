// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1300:Element should begin with upper-case letter",
        Justification = "Reviewed")]
    public enum Platform
    {
        Unknown,
        Android,
        iOS,
        WindowsPhone,
        Windows,
        WindowsTablet,
        SurfaceHub,
        Xbox,
        IoT,
        tvOS,
        watchOS,
        macOS,
        Tizen
    }
}