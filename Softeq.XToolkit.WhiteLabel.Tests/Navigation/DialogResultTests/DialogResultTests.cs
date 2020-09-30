// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.DialogResultTests
{
    public class DialogResultTests
    {
        [Theory]
        [MemberData(
            nameof(DialogResultDataProvider.CtorData),
            MemberType = typeof(DialogResultDataProvider))]
        public void Ctor_InitializesProperties(Task<bool> dialogDismissTask)
        {
            var dialogResult = new DialogResult(dialogDismissTask);

            Assert.IsAssignableFrom<IDialogResult>(dialogResult);
            Assert.Equal(dialogDismissTask, dialogResult.WaitDismissAsync);
        }

        [Theory]
        [MemberData(
            nameof(DialogResultDataProvider.GenericCtorData),
            MemberType = typeof(DialogResultDataProvider))]
        public void Ctor_Generic_InitializesProperties(string value, Task<bool> dialogDismissTask)
        {
            var dialogResult = new DialogResult<string>(value, dialogDismissTask);

            Assert.IsAssignableFrom<IDialogResult<string>>(dialogResult);
            Assert.Equal(dialogDismissTask, dialogResult.WaitDismissAsync);
            Assert.Equal(value, dialogResult.Value);
        }
    }
}
