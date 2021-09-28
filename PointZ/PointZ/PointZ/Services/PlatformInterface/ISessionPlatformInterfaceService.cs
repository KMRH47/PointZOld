using PointZ.Models.DisplayDimensions;
using PointZ.Models.NavigationBarDimension;

namespace PointZ.Services.PlatformInterface
{
    public interface ISessionPlatformInterfaceService
    {
        DisplayDimensionData GetDisplayDimensions();
        NavigationBarDimensionData GetNavigationBarDimensions();
        void ToggleKeyboard();
    }
}