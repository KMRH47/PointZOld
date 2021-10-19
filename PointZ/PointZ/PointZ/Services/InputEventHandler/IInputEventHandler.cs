using System.Threading.Tasks;

namespace PointZ.Services.InputEventHandler
{
    public interface IInputEventHandler<in TEventArgs>
    {
        Task HandleAsync(TEventArgs e);
    }
}