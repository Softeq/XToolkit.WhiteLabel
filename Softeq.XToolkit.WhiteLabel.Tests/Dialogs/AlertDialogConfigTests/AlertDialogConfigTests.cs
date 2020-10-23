// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Dialogs.AlertDialogConfigTests
{
    public class AlertDialogConfigTests
    {
        private const string EmptyString = "";
        private const string NonEmptyTitle = "Abc Title";
        private const string NonEmptyMessage = "Abc Message";
        private const string NonEmptyCloseButtonText = "Abc Cancel Button";

        [Theory]
        [PairwiseData]
        public void Ctor_NullTitle_ThrowsCorrectException(
            [CombinatorialValues(null, EmptyString, NonEmptyMessage)] string message,
            [CombinatorialValues(null, EmptyString, NonEmptyCloseButtonText)] string closeButtonText)
        {
            Assert.Throws<ArgumentNullException>(()
                => new AlertDialogConfig(null, message, closeButtonText));
        }

        [Theory]
        [PairwiseData]
        public void Ctor_NonNullTitle_NullMessage_ThrowsCorrectException(
            [CombinatorialValues(EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(null, EmptyString, NonEmptyCloseButtonText)] string closeButtonText)
        {
            Assert.Throws<ArgumentNullException>(()
                => new AlertDialogConfig(title, null, closeButtonText));
        }

        [Theory]
        [PairwiseData]
        public void Ctor_NonNullTitle_NonNullMessage_NullCloseButtonText_ThrowsCorrectException(
            [CombinatorialValues(EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(EmptyString, NonEmptyMessage)] string message)
        {
            Assert.Throws<ArgumentNullException>(()
                => new AlertDialogConfig(title, message, null));
        }

        [Theory]
        [PairwiseData]
        public void Ctor_NonNullTitle_NonNullMessage_NonNullCloseButtonText_FillsProperties(
            [CombinatorialValues(EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(EmptyString, NonEmptyMessage)] string message,
            [CombinatorialValues(EmptyString, NonEmptyCloseButtonText)] string closeButtonText)
        {
            var alertDialogConfig =
                new AlertDialogConfig(title, message, closeButtonText);

            Assert.Equal(title, alertDialogConfig.Title);
            Assert.Equal(message, alertDialogConfig.Message);
            Assert.Equal(closeButtonText, alertDialogConfig.CloseButtonText);
        }
    }
}
