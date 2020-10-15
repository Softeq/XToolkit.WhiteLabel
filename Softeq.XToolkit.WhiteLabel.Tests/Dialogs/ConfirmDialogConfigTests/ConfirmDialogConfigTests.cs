// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Dialogs.ConfirmDialogConfigTests
{
    public class ConfirmDialogConfigTests
    {
        private const string EmptyString = "";
        private const string NonEmptyTitle = "Abc Title";
        private const string NonEmptyMessage = "Abc Message";
        private const string NonEmptyAcceptButtonText = "Abc Accept Button";
        private const string NonEmptyCancelButtonText = "Abc Cancel Button";

        [Theory]
        [PairwiseData]
        public void Ctor_NullTitle_ThrowsCorrectException(
            [CombinatorialValues(null, EmptyString, NonEmptyMessage)] string message,
            [CombinatorialValues(null, EmptyString, NonEmptyAcceptButtonText)] string acceptButtonText,
            [CombinatorialValues(null, EmptyString, NonEmptyCancelButtonText)] string cancelButtonText,
            [CombinatorialValues(true, false)] bool isDestructive)
        {
            Assert.Throws<ArgumentNullException>(()
                => new ConfirmDialogConfig(null, message, acceptButtonText, cancelButtonText, isDestructive));
        }

        [Theory]
        [PairwiseData]
        public void Ctor_NonNullTitle_NullMessage_ThrowsCorrectException(
            [CombinatorialValues(EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(null, EmptyString, NonEmptyAcceptButtonText)] string acceptButtonText,
            [CombinatorialValues(null, EmptyString, NonEmptyCancelButtonText)] string cancelButtonText,
            [CombinatorialValues(true, false)] bool isDestructive)
        {
            Assert.Throws<ArgumentNullException>(()
                => new ConfirmDialogConfig(title, null, acceptButtonText, cancelButtonText, isDestructive));
        }

        [Theory]
        [PairwiseData]
        public void Ctor_NonNullTitle_NonNullMessage_NullAcceptButtonText_ThrowsCorrectException(
            [CombinatorialValues(EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(EmptyString, NonEmptyMessage)] string message,
            [CombinatorialValues(null, EmptyString, NonEmptyCancelButtonText)] string cancelButtonText,
            [CombinatorialValues(true, false)] bool isDestructive)
        {
            Assert.Throws<ArgumentNullException>(()
                => new ConfirmDialogConfig(title, message, null, cancelButtonText, isDestructive));
        }

        [Theory]
        [PairwiseData]
        public void Ctor_NonNullTitle_NonNullMessage_NonNullAcceptButtonText_FillsProperties(
            [CombinatorialValues(EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(EmptyString, NonEmptyMessage)] string message,
            [CombinatorialValues(EmptyString, NonEmptyAcceptButtonText)] string acceptButtonText,
            [CombinatorialValues(null, EmptyString, NonEmptyCancelButtonText)] string cancelButtonText,
            [CombinatorialValues(true, false)] bool isDestructive)
        {
            var confirmDialogConfig =
                new ConfirmDialogConfig(title, message, acceptButtonText, cancelButtonText, isDestructive);

            Assert.Equal(title, confirmDialogConfig.Title);
            Assert.Equal(message, confirmDialogConfig.Message);
            Assert.Equal(acceptButtonText, confirmDialogConfig.AcceptButtonText);
            Assert.Equal(cancelButtonText, confirmDialogConfig.CancelButtonText);
            Assert.Equal(isDestructive, confirmDialogConfig.IsDestructive);
        }
    }
}
