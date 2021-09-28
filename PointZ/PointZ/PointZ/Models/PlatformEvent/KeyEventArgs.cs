namespace PointZ.Models.PlatformEvent
{
    public class KeyEventArgs
    {
        public KeyEventArgs(KeyAction keyAction, string keyCode)
        {
            KeyAction = keyAction;
            KeyCode = keyCode;
        }

        public KeyAction KeyAction { get; }
        public string KeyCode { get; }
    }
}