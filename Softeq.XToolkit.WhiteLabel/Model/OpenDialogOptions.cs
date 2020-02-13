// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public enum DialogType
    {
        Default,
        Destructive
    }

    public class OpenDialogOptions
    {
        [Obsolete("This was used when iOS 10 appeared and there were some issues " +
            "with dialogs background. It should be deleted as it's not used anymore.")]
        public bool ShouldShowBackgroundOverlay { get; set; }

        public DialogType DialogType { get; set; }
    }
}