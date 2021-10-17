using System.Threading.Tasks;
using PointZ.Models.Command;

namespace PointZ.Services.InputCommandSender
{
    public interface IMouseCommandSender 
    {
        Task SendMouseCommandAsync(MouseCommand command);
        Task MoveMouseByAsync(int x, int y);
        Task MoveMouseToAsync(int x, int y);
        Task MoveMouseToPositionOnVirtualDesktopAsync(int x, int y);
        Task HorizontalScrollAsync(int amount);
        Task VerticalScrollAsync(int amount);
    }
}