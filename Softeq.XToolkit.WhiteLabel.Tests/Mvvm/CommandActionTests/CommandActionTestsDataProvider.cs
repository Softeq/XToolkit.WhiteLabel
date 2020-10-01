// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Mvvm.CommandActionTests
{
    internal static class CommandActionTestsDataProvider
    {
        private static readonly ICommand CommandSubstitute = Substitute.For<ICommand>();

        public static TheoryData<ICommand, string, CommandActionStyle> CtorData
           => new TheoryData<ICommand, string, CommandActionStyle>
           {
               { null, null, CommandActionStyle.Default },
               { null, string.Empty, CommandActionStyle.Destructive },
               { CommandSubstitute, null, CommandActionStyle.Cancel },
               { CommandSubstitute, string.Empty, CommandActionStyle.Default },
           };
    }
}
