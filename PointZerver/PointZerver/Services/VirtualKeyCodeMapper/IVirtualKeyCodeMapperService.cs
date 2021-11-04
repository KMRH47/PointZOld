using InputSimulatorStandard.Native;

namespace PointZerver.Services.VirtualKeyCodeMapper
{
    public interface IVirtualKeyCodeMapperService
    {
        VirtualKeyCode ParseString(string keycodeString);
    }
}