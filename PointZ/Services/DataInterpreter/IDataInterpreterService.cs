using System.Threading.Tasks;

namespace PointZ.Services.DataInterpreter
{
    public interface IDataInterpreterService
    {
        Task InterpretAsync(byte[] bytes);
}
}