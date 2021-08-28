using System;

namespace PointZ.Models.Server
{
    public readonly struct ServerData
    {
        public ServerData(string name, string address)
        {
            Name = name;
            Address = address;
            LastUpdated = DateTime.Now;
        }

        public string Name { get; }
        public string Address { get; }
        public DateTime LastUpdated { get; }
    }
}