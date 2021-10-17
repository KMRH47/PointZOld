using PointZ.Models.DisplayDimensions;

namespace PointZ.Services.PlatformSettings
{
    public interface IPlatformSettingsService
    {
        float DisplayDensity { get;  set; }
        DisplayDimensionData GetDisplayDimensions();
        void ToggleKeyboard();
    }
}