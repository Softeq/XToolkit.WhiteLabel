// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Dialogs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Dialogs.ActionSheetDialogConfigTests
{
    public class ActionSheetDialogConfigTests
    {
        private const string EmptyString = "";
        private const string NonEmptyTitle = "Abc Title";
        private const string NonEmptyCancelButtonText = "Abc Cancel Button";
        private const string NonEmptyDestructButtonText = "Abc Destruct Button";

        [Theory]
        [PairwiseData]
        public void Ctor_WithNullOptions_CreatesEmptyOptionsAndFillsOtherProperties(
            [CombinatorialValues(null, EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(null, EmptyString, NonEmptyCancelButtonText)] string cancelButtonText,
            [CombinatorialValues(null, EmptyString, NonEmptyDestructButtonText)] string destructButtonText)
        {
            var actionSheetDialogConfig =
                new ActionSheetDialogConfig(null, title, cancelButtonText, destructButtonText);

            Assert.NotNull(actionSheetDialogConfig.OptionButtons);
            Assert.Empty(actionSheetDialogConfig.OptionButtons);

            Assert.Equal(title, actionSheetDialogConfig.Title);
            Assert.Equal(cancelButtonText, actionSheetDialogConfig.CancelButtonText);
            Assert.Equal(destructButtonText, actionSheetDialogConfig.DestructButtonText);
        }

        [Theory]
        [PairwiseData]
        public void Ctor_WithEmptyOptions_FillsProperties(
            [CombinatorialValues(null, EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(null, EmptyString, NonEmptyCancelButtonText)] string cancelButtonText,
            [CombinatorialValues(null, EmptyString, NonEmptyDestructButtonText)] string destructButtonText)
        {
            var options = new string[] { };
            var actionSheetDialogConfig =
                new ActionSheetDialogConfig(options, title, cancelButtonText, destructButtonText);

            Assert.Equal(options, actionSheetDialogConfig.OptionButtons);
            Assert.Equal(title, actionSheetDialogConfig.Title);
            Assert.Equal(cancelButtonText, actionSheetDialogConfig.CancelButtonText);
            Assert.Equal(destructButtonText, actionSheetDialogConfig.DestructButtonText);
        }

        [Theory]
        [PairwiseData]
        public void Ctor_WithNonEmptyOptions_FillsProperties(
            [CombinatorialValues(null, EmptyString, NonEmptyTitle)] string title,
            [CombinatorialValues(null, EmptyString, NonEmptyCancelButtonText)] string cancelButtonText,
            [CombinatorialValues(null, EmptyString, NonEmptyDestructButtonText)] string destructButtonText)
        {
            var options = new string[] { "Option1", "Option2", "Option3" };
            var actionSheetDialogConfig =
                new ActionSheetDialogConfig(options, title, cancelButtonText, destructButtonText);

            Assert.Equal(options, actionSheetDialogConfig.OptionButtons);
            Assert.Equal(title, actionSheetDialogConfig.Title);
            Assert.Equal(cancelButtonText, actionSheetDialogConfig.CancelButtonText);
            Assert.Equal(destructButtonText, actionSheetDialogConfig.DestructButtonText);
        }
    }
}
