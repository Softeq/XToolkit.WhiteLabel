// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Mvvm.DialogViewModelComponentTests
{
    public class DialogViewModelComponentTests
    {
        [Fact]
        public void Ctor_InitializesProperties()
        {
            var dialogViewModelComponent = new DialogViewModelComponent();

            Assert.NotNull(dialogViewModelComponent.CloseCommand);
            Assert.NotNull(dialogViewModelComponent.Task);
        }

        [Fact]
        public void CloseCommand_CompletesTaskWithCorrectResultAndNotifies()
        {
            var param = new object();
            var dialogViewModelComponent = new DialogViewModelComponent();

            var raisedEvent = Assert.Raises<EventArgs>(
                    handler => dialogViewModelComponent.Closed += (s, e) => handler.Invoke(s, e),
                    handler => { },
                    () => dialogViewModelComponent.CloseCommand.Execute(param));

            Assert.NotNull(raisedEvent);
            Assert.True(dialogViewModelComponent.Task.IsCompleted);
            Assert.Equal(param, dialogViewModelComponent.Task.Result);
        }
    }
}
