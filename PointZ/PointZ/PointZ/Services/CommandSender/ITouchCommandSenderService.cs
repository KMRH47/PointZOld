using System.Threading.Tasks;
using PointZ.Services.CommandSender.Base;

namespace PointZ.Services.CommandSender
{
    public interface ITouchCommandSenderService 
    {
        Task MoveMouseByAsync(int x, int y);
        Task MoveMouseToAsync(int x, int y);
        Task MoveMouseToPositionOnVirtualDesktopAsync(int x, int y);
        Task HorizontalScrollAsync(int amount);
        Task VerticalScrollAsync(int amount);
    }
}