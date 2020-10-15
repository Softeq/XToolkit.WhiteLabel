// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using System.Reflection;

namespace Softeq.XToolkit.Remote.Tests.Auth.Handlers.RefreshTokenHttpClientHandlerTests
{
    internal static class SubstituteExtensions
    {
        public static object? Protected(this object target, string name, params object[] args)
        {
            var type = target.GetType();
            var method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Single(x => x.Name == name);
            return method.Invoke(target, args);
        }
    }
}
