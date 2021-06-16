using System.Threading.Tasks;

namespace PointZ.Services.DataInterpreter
{
    public interface IDataInterpreter
    {
        Task InterpretAsync(byte[] bytes);
}
}