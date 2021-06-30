using System;
using System.Net;
using System.Net.Sockets;

namespace PointZ.Extensions
{
    public static class UdpClientExtensions
    {
        /// <summary>
        /// Gets the port that this UdpClient is currently bound to.
        /// </summary>
        /// <returns>The port that this UdpClient is currently bound to.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static int GetBoundPort(this UdpClient udpClient)
        {
            if (udpClient.Client.LocalEndPoint is IPEndPoint ipEndPoint)
                return ipEndPoint.Port;

            throw new NullReferenceException($"{udpClient} is not bound to an address.");
        }
    }
}