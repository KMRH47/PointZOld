using System;
using System.Runtime.InteropServices;

namespace PointZerver.Services.Simulators.Controllers
{
    public class MacMouseController : IMouseController
    {
        private const string ApplicationServicesFramework = "/System/Library/Frameworks/ApplicationServices.framework/Versions/A/ApplicationServices";

        private bool leftButtonDown;
        private bool rightButtonDown;
        private bool middleButtonDown;

        public void HorizontalScroll(int amount)
        {
            int delta = -amount;
            IntPtr scrollEvent = CGEventCreateScrollWheelEvent(IntPtr.Zero, CGScrollEventUnit.Line, 2, 0, delta, 0);
            PostEvent(scrollEvent);
        }

        public void VerticalScroll(int amount)
        {
            int delta = -amount;
            IntPtr scrollEvent = CGEventCreateScrollWheelEvent(IntPtr.Zero, CGScrollEventUnit.Line, 1, delta, 0, 0);
            PostEvent(scrollEvent);
        }

        public void LeftButtonClick()
        {
            LeftButtonDown();
            LeftButtonUp();
        }

        public void LeftButtonDown()
        {
            this.leftButtonDown = true;
            PostMouseEvent(CGEventType.LeftMouseDown, CGMouseButton.Left, GetCurrentMousePosition());
        }

        public void LeftButtonUp()
        {
            PostMouseEvent(CGEventType.LeftMouseUp, CGMouseButton.Left, GetCurrentMousePosition());
            this.leftButtonDown = false;
        }

        public void MiddleButtonClick()
        {
            MiddleButtonDown();
            MiddleButtonUp();
        }

        public void MiddleButtonDown()
        {
            this.middleButtonDown = true;
            PostMouseEvent(CGEventType.OtherMouseDown, CGMouseButton.Center, GetCurrentMousePosition());
        }

        public void MiddleButtonUp()
        {
            PostMouseEvent(CGEventType.OtherMouseUp, CGMouseButton.Center, GetCurrentMousePosition());
            this.middleButtonDown = false;
        }

        public void RightButtonClick()
        {
            RightButtonDown();
            RightButtonUp();
        }

        public void RightButtonDown()
        {
            this.rightButtonDown = true;
            PostMouseEvent(CGEventType.RightMouseDown, CGMouseButton.Right, GetCurrentMousePosition());
        }

        public void RightButtonUp()
        {
            PostMouseEvent(CGEventType.RightMouseUp, CGMouseButton.Right, GetCurrentMousePosition());
            this.rightButtonDown = false;
        }

        public void MoveMouseBy(int x, int y)
        {
            CGPoint currentPosition = GetCurrentMousePosition();
            CGPoint targetPosition = new(currentPosition.X + x, currentPosition.Y - y);
            PostMouseMove(targetPosition);
        }

        public void MoveMouseTo(double x, double y)
        {
            CGPoint targetPosition = CalculateAbsolutePosition(x, y);
            PostMouseMove(targetPosition);
        }

        public void MoveMouseToPositionOnVirtualDesktop(double x, double y)
        {
            CGPoint targetPosition = CalculateAbsolutePosition(x, y);
            PostMouseMove(targetPosition);
        }

        private static void PostEvent(IntPtr eventRef)
        {
            if (eventRef == IntPtr.Zero) return;

            CGEventPost(CGEventTapLocation.Hid, eventRef);
            CFRelease(eventRef);
        }

        private void PostMouseMove(CGPoint position)
        {
            CGEventType eventType = this.leftButtonDown
                ? CGEventType.LeftMouseDragged
                : this.rightButtonDown
                    ? CGEventType.RightMouseDragged
                    : this.middleButtonDown
                        ? CGEventType.OtherMouseDragged
                        : CGEventType.MouseMoved;
            IntPtr mouseEvent = CGEventCreateMouseEvent(IntPtr.Zero, eventType, position, CGMouseButton.Left);
            PostEvent(mouseEvent);
        }

        private static void PostMouseEvent(CGEventType eventType, CGMouseButton button, CGPoint position)
        {
            IntPtr mouseEvent = CGEventCreateMouseEvent(IntPtr.Zero, eventType, position, button);
            PostEvent(mouseEvent);
        }

        private static CGPoint GetCurrentMousePosition()
        {
            IntPtr eventRef = CGEventCreate(IntPtr.Zero);
            CGPoint position = CGEventGetLocation(eventRef);
            CFRelease(eventRef);
            return position;
        }

        private static CGPoint CalculateAbsolutePosition(double normalizedX, double normalizedY)
        {
            CGRect bounds = CGDisplayBounds(CGMainDisplayID());
            double absoluteX = bounds.Origin.X + (normalizedX / 65535d) * bounds.Size.Width;
            double absoluteY = bounds.Origin.Y + bounds.Size.Height - (normalizedY / 65535d) * bounds.Size.Height;
            return new CGPoint(absoluteX, absoluteY);
        }

        [DllImport(ApplicationServicesFramework)]
        private static extern IntPtr CGEventCreateMouseEvent(IntPtr source, CGEventType mouseType, CGPoint mouseCursorPosition, CGMouseButton mouseButton);

        [DllImport(ApplicationServicesFramework)]
        private static extern IntPtr CGEventCreateScrollWheelEvent(IntPtr source, CGScrollEventUnit units, uint wheelCount, int wheel1, int wheel2, int wheel3);

        [DllImport(ApplicationServicesFramework)]
        private static extern void CGEventPost(CGEventTapLocation tap, IntPtr @event);

        [DllImport(ApplicationServicesFramework)]
        private static extern void CFRelease(IntPtr cfTypeRef);

        [DllImport(ApplicationServicesFramework)]
        private static extern IntPtr CGEventCreate(IntPtr source);

        [DllImport(ApplicationServicesFramework)]
        private static extern CGPoint CGEventGetLocation(IntPtr @event);

        [DllImport(ApplicationServicesFramework)]
        private static extern uint CGMainDisplayID();

        [DllImport(ApplicationServicesFramework)]
        private static extern CGRect CGDisplayBounds(uint display);

        private enum CGEventType
        {
            LeftMouseDown = 1,
            LeftMouseUp = 2,
            RightMouseDown = 3,
            RightMouseUp = 4,
            MouseMoved = 5,
            LeftMouseDragged = 6,
            RightMouseDragged = 7,
            OtherMouseDown = 25,
            OtherMouseUp = 26,
            OtherMouseDragged = 27
        }

        private enum CGMouseButton
        {
            Left = 0,
            Right = 1,
            Center = 2
        }

        private enum CGEventTapLocation
        {
            Hid = 0
        }

        private enum CGScrollEventUnit
        {
            Pixel = 0,
            Line = 1
        }

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct CGPoint
        {
            public readonly double X;
            public readonly double Y;

            public CGPoint(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct CGSize
        {
            public readonly double Width;
            public readonly double Height;

            public CGSize(double width, double height)
            {
                this.Width = width;
                this.Height = height;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct CGRect
        {
            public readonly CGPoint Origin;
            public readonly CGSize Size;

            public CGRect(CGPoint origin, CGSize size)
            {
                this.Origin = origin;
                this.Size = size;
            }
        }
    }
}
