using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PointZClient.Tools
{
    public static class NetworkTools
    {
        /// <summary>
        /// Attempts to get the local IPv4 address from the primary NIC.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task<string> GetLocalIpv4Address()
        {
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            await socket.ConnectAsync(IPAddress.Broadcast, 65530);
            
            if (socket.LocalEndPoint is not IPEndPoint localEndPoint)
                throw new NullReferenceException();
            
            socket.Dispose();
            
            return localEndPoint.Address.ToString();
        }
    }
}