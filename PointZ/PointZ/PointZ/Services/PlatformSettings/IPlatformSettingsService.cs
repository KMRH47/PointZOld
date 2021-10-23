namespace PointZ.Services.PlatformSettings
{
    public interface IPlatformSettingsService
    {
        float DisplayDensity { get; }
        void SetSoftInputModeAdjustResize();
        void DisplayPopupHint(string message);
    }
}