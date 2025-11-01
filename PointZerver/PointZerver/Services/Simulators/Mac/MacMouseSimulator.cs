using System;
using System.Runtime.InteropServices;

namespace PointZerver.Services.Simulators.Mac
{
    public interface IMouseSimulator
    {
        void MoveMouseBy(int pixelDeltaX, int pixelDeltaY);
        void MoveMouseTo(double absoluteX, double absoluteY);
        void MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY);
        void LeftButtonDown();
        void LeftButtonUp();
        void LeftButtonClick();
        void RightButtonDown();
        void RightButtonUp();
        void RightButtonClick();
        void MiddleButtonDown();
        void MiddleButtonUp();
        void MiddleButtonClick();
        void VerticalScroll(int scrollAmountInClicks);
        void HorizontalScroll(int scrollAmountInClicks);
    }

    public class MacMouseSimulator : IMouseSimulator
    {
        // CoreGraphics P/Invoke declarations
        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern IntPtr CGEventCreateMouseEvent(IntPtr source, uint mouseType, CGPoint mouseCursorPosition, uint mouseButton);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern void CGEventPost(uint tap, IntPtr cgEvent);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern void CFRelease(IntPtr cfTypeRef);

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern CGPoint CGEventGetLocation(IntPtr eventRef);

        [StructLayout(LayoutKind.Sequential)]
        private struct CGPoint
        {
            public double X;
            public double Y;
        }

        private const uint kCGEventMouseMoved = 5;
        private const uint kCGEventLeftMouseDown = 1;
        private const uint kCGEventLeftMouseUp = 2;
        private const uint kCGEventRightMouseDown = 3;
        private const uint kCGEventRightMouseUp = 4;
        private const uint kCGEventOtherMouseDown = 25;
        private const uint kCGEventOtherMouseUp = 26;
        private const uint kCGMouseButtonLeft = 0;
        private const uint kCGMouseButtonRight = 1;
        private const uint kCGMouseButtonCenter = 2;
        private const uint kCGHIDEventTap = 0;

        private CGPoint GetCurrentMousePosition()
        {
            IntPtr eventRef = CGEventCreateMouseEvent(IntPtr.Zero, kCGEventMouseMoved, new CGPoint { X = 0, Y = 0 }, 0);
            CGPoint location = CGEventGetLocation(eventRef);
            CFRelease(eventRef);
            return location;
        }

        public void MoveMouseBy(int pixelDeltaX, int pixelDeltaY)
        {
            CGPoint currentPos = GetCurrentMousePosition();
            MoveMouseTo(currentPos.X + pixelDeltaX, currentPos.Y + pixelDeltaY);
        }

        public void MoveMouseTo(double absoluteX, double absoluteY)
        {
            CGPoint point = new CGPoint { X = absoluteX, Y = absoluteY };
            IntPtr eventRef = CGEventCreateMouseEvent(IntPtr.Zero, kCGEventMouseMoved, point, 0);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        public void MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY)
        {
            MoveMouseTo(absoluteX, absoluteY);
        }

        public void LeftButtonDown()
        {
            CGPoint currentPos = GetCurrentMousePosition();
            IntPtr eventRef = CGEventCreateMouseEvent(IntPtr.Zero, kCGEventLeftMouseDown, currentPos, kCGMouseButtonLeft);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        public void LeftButtonUp()
        {
            CGPoint currentPos = GetCurrentMousePosition();
            IntPtr eventRef = CGEventCreateMouseEvent(IntPtr.Zero, kCGEventLeftMouseUp, currentPos, kCGMouseButtonLeft);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        public void LeftButtonClick()
        {
            LeftButtonDown();
            LeftButtonUp();
        }

        public void RightButtonDown()
        {
            CGPoint currentPos = GetCurrentMousePosition();
            IntPtr eventRef = CGEventCreateMouseEvent(IntPtr.Zero, kCGEventRightMouseDown, currentPos, kCGMouseButtonRight);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        public void RightButtonUp()
        {
            CGPoint currentPos = GetCurrentMousePosition();
            IntPtr eventRef = CGEventCreateMouseEvent(IntPtr.Zero, kCGEventRightMouseUp, currentPos, kCGMouseButtonRight);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        public void RightButtonClick()
        {
            RightButtonDown();
            RightButtonUp();
        }

        public void MiddleButtonDown()
        {
            CGPoint currentPos = GetCurrentMousePosition();
            IntPtr eventRef = CGEventCreateMouseEvent(IntPtr.Zero, kCGEventOtherMouseDown, currentPos, kCGMouseButtonCenter);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        public void MiddleButtonUp()
        {
            CGPoint currentPos = GetCurrentMousePosition();
            IntPtr eventRef = CGEventCreateMouseEvent(IntPtr.Zero, kCGEventOtherMouseUp, currentPos, kCGMouseButtonCenter);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        public void MiddleButtonClick()
        {
            MiddleButtonDown();
            MiddleButtonUp();
        }

        public void VerticalScroll(int scrollAmountInClicks)
        {
            CGPoint currentPos = GetCurrentMousePosition();
            IntPtr eventRef = CGEventCreateScrollWheelEvent(IntPtr.Zero, 1, scrollAmountInClicks, 0);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        public void HorizontalScroll(int scrollAmountInClicks)
        {
            CGPoint currentPos = GetCurrentMousePosition();
            IntPtr eventRef = CGEventCreateScrollWheelEvent(IntPtr.Zero, 1, 0, scrollAmountInClicks);
            CGEventPost(kCGHIDEventTap, eventRef);
            CFRelease(eventRef);
        }

        [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
        private static extern IntPtr CGEventCreateScrollWheelEvent(IntPtr source, uint units, int wheelCount, int wheel1, int wheel2 = 0, int wheel3 = 0);
    }
}
