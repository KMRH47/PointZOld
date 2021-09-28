using InputSimulatorStandard.Native;

namespace PointZerver.Services.CommandConverter
{
    public interface IVirtualKeyCodeConverterService
    {
        VirtualKeyCode ParseString(string keycodeString);
    }
}