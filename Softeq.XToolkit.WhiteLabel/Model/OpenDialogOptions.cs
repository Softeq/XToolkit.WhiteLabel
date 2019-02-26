// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public enum DialogType
    {
        Default,
        Destructive
    }

    public class OpenDialogOptions
    {
        public bool ShouldShowBackgroundOverlay { get; set; }
        public DialogType DialogType { get; set; }
    }
}