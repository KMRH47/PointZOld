namespace PointZ.Models.KeyEvent
{
    public class KeyEventArgs
    {
        public KeyEventArgs(KeyAction keyAction, KeyCodeAction keyCodeAction)
        {
            KeyAction = keyAction;
            KeyCodeAction = keyCodeAction;
        }

        public KeyAction KeyAction { get; }
        public KeyCodeAction KeyCodeAction { get; }
    }
}