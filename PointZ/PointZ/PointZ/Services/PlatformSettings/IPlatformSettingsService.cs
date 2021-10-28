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
        /// <param name="duration">0 = short, anything else = long.</param>
        void DisplayPopupHint(string message, byte duration);
    }
}