using PointZ.Models.DisplayDimensions;

namespace PointZ.Services.PlatformInterface
{
    public interface IPlatformInterfaceService
    {
        
        
        float DisplayDensity { get;  set; }
        DisplayDimensionData GetDisplayDimensions();
        void ToggleKeyboard();
    }
}