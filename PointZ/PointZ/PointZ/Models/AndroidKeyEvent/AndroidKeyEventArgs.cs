namespace PointZ.Models.AndroidKeyEvent
{
    public class AndroidKeyEventArgs
    {
        public AndroidKeyEventArgs(AndroidKeyAction androidKeyAction, AndroidKeyCodeAction androidKeyCodeAction)
        {
            AndroidKeyAction = androidKeyAction;
            AndroidKeyCodeAction = androidKeyCodeAction;
        }

        public AndroidKeyAction AndroidKeyAction { get; }
        public AndroidKeyCodeAction AndroidKeyCodeAction { get; }
    }
}