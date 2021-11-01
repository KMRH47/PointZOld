using System.Net;

namespace PointZ.Models.Server
{
    public class ServerData
    {
        public ServerData(string name, IPEndPoint ipEndPoint)
        {
            Name = name;
            IpEndPoint = ipEndPoint;
        }

        public string Name { get; }
        public IPEndPoint IpEndPoint { get; }
    }
}