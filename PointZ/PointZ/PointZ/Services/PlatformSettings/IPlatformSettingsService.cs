using PointZ.Models.DisplayDimensions;
using PointZ.Models.SoftInput;

namespace PointZ.Services.PlatformSettings
{
    public interface IPlatformSettingsService
    {
        float DisplayDensity { get;  set; }
        DisplayDimensionData GetDisplayDimensions();
        void ToggleKeyboard();
        void WindowSoftInputMode(SoftInput softInput);
    }
}