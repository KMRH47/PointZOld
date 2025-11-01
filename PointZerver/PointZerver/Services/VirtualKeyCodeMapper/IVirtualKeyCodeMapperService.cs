using SharpHook.Native;

namespace PointZerver.Services.VirtualKeyCodeMapper
{
    public interface IVirtualKeyCodeMapperService
    {
        KeyCode ParseString(string keycodeString);
    }
}
