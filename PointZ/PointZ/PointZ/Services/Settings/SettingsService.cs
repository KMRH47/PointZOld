using System.Net;

namespace PointZ.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public IPEndPoint ServerIpEndPoint { get; set; }
        public int TapDelayMs { get; set; } = 150;
        public int DoubleTapDelayMs { get; set; } = 200;
        public int DeadZoneInitial { get; set; } = 25;
        public int DeadZoneScroll { get; set; } = 40;
        public byte ScrollSpeed { get; set; } = 1;
        public bool LeftMouseButtonPrimary { get; set; } = true;
    }
}