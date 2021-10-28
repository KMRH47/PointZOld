namespace PointZ.Models.TouchEvent
{
    public class TouchEventArgs
    {
        public TouchEventArgs(float x, float y, TouchAction touchAction)
        {
            X = x;
            Y = y;
            TouchAction = touchAction;
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
        public TouchAction TouchAction { get; }
    }
}