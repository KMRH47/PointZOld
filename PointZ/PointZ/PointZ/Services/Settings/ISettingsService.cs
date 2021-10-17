using System.Net;

namespace PointZ.Services.Settings
{
    public interface ISettingsService
    {
        public IPEndPoint ServerIpEndPoint { get; set; }
        public int TapDelayMs { get; set; }
        public int DoubleTapDelayMs { get; set; }
        public int DeadZoneInitial { get; set; } 
        public int DeadZoneScroll { get; set; }
        public byte ScrollSpeed { get; set; }
        public bool LeftMouseButtonPrimary { get; set; } 
    }
}