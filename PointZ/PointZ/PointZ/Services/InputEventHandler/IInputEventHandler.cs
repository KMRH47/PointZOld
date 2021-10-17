using System.Threading.Tasks;

namespace PointZ.Services.InputEventHandler
{
    public interface IInputCommandSender<in TEventArgs>
    {
        Task HandleAsync(TEventArgs e);
    }
}