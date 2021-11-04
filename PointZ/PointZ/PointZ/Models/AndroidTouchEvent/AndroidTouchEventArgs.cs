namespace PointZ.Models.AndroidTouchEvent
{
    public class AndroidTouchEventArgs
    {
        public AndroidTouchEventArgs(float x, float y, AndroidTouchAction androidTouchAction)
        {
            X = x;
            Y = y;
            AndroidTouchAction = androidTouchAction;
        }

        /// <summary>
        /// The current position of the cursor on the X-axis.
        /// </summary>
        public float X { get; }
        
        /// <summary>
        /// The current position of the cursor on the Y-axis.
        /// </summary>
        public float Y { get; }
        
        /// <summary>
        /// The touch action associated with this event.
        /// </summary>
        public AndroidTouchAction AndroidTouchAction { get; }
    }
}