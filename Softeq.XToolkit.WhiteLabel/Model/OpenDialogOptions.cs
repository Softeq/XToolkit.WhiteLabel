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

        public bool TopLevel { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string OkButtonText { get; set; } = "OK";
        public string CancelButtonText { get; set; } = "Cancel";

    }
}