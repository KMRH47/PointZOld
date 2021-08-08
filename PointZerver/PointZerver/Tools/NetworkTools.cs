using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PointZerver.Tools
{
    public static class NetworkTools
    {
        /// <summary>
        /// Attempts to get the local IPv4 address from the primary NIC.
        /// </summary>
        /// <param name="token">Token parameter for cancellation support.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task<string> GetLocalIpv4Address(CancellationToken token)
        {
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            await socket.ConnectAsync(IPAddress.Broadcast, 65530, token);
            
            if (socket.LocalEndPoint is not IPEndPoint localEndPoint)
                throw new NullReferenceException();
            
            socket.Dispose();
            
            return localEndPoint.Address.ToString();
        }
    }
}