namespace PointZ.Services.PlatformSettings
{
    public interface IPlatformSettingsService
    {
        float DisplayDensity { get; }
        void SetSoftInputModeAdjustResize();
        /// <summary>
        /// Displays a pop-up hint. 
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="isQuickPopup">True = short duration, false = longer duration.</param>
        void DisplayPopupHint(string message, bool isQuickPopup);
    }
}