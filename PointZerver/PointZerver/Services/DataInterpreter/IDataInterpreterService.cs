using System.Threading.Tasks;

namespace PointZerver.Services.DataInterpreter
{
    public interface IDataInterpreterService
    {
        Task InterpretAsync(byte[] bytes);
    }
}