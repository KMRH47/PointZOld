using System.Net.Sockets;
using System.Threading.Tasks;

namespace PointZTest
{
    public class ClientAuthTests
    {
        private readonly TcpClient tcpClient;

        public ClientAuthTests(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public async Task CanAuthorizeClientAsync()
        {
        }
    }
}