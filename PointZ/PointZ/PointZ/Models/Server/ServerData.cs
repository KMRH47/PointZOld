using System;
using System.Net;

namespace PointZ.Models.Server
{
    public readonly struct ServerData
    {
        public ServerData(string name, IPEndPoint ipEndPoint)
        {
            Name = name;
            IpEndPoint = ipEndPoint;
            LastUpdated = DateTime.Now;
        }

        public string Name { get; }
        public IPEndPoint IpEndPoint { get; }
        public DateTime LastUpdated { get; }
    }
}