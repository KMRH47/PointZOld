using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PointZerver.Services.VirtualKeyCodeMapper;

namespace PointZerver.Services.Simulators.Controllers
{
    public class MacKeyboardController : IKeyboardController
    {
        private const string ApplicationServicesFramework = "/System/Library/Frameworks/ApplicationServices.framework/Versions/A/ApplicationServices";

        private static readonly IReadOnlyDictionary<KeycodeAction, ushort> KeycodeMap = new Dictionary<KeycodeAction, ushort>
        {
            { KeycodeAction.A, 0x00 },
            { KeycodeAction.B, 0x0B },
            { KeycodeAction.C, 0x08 },
            { KeycodeAction.D, 0x02 },
            { KeycodeAction.E, 0x0E },
            { KeycodeAction.F, 0x03 },
            { KeycodeAction.G, 0x05 },
            { KeycodeAction.H, 0x04 },
            { KeycodeAction.I, 0x22 },
            { KeycodeAction.J, 0x26 },
            { KeycodeAction.K, 0x28 },
            { KeycodeAction.L, 0x25 },
            { KeycodeAction.M, 0x2E },
            { KeycodeAction.N, 0x2D },
            { KeycodeAction.O, 0x1F },
            { KeycodeAction.P, 0x23 },
            { KeycodeAction.Q, 0x0C },
            { KeycodeAction.R, 0x0F },
            { KeycodeAction.S, 0x01 },
            { KeycodeAction.T, 0x11 },
            { KeycodeAction.U, 0x20 },
            { KeycodeAction.V, 0x09 },
            { KeycodeAction.W, 0x0D },
            { KeycodeAction.X, 0x07 },
            { KeycodeAction.Y, 0x10 },
            { KeycodeAction.Z, 0x06 },
            { KeycodeAction.Num1, 0x12 },
            { KeycodeAction.Num2, 0x13 },
            { KeycodeAction.Num3, 0x14 },
            { KeycodeAction.Num4, 0x15 },
            { KeycodeAction.Num5, 0x17 },
            { KeycodeAction.Num6, 0x16 },
            { KeycodeAction.Num7, 0x1A },
            { KeycodeAction.Num8, 0x1C },
            { KeycodeAction.Num9, 0x19 },
            { KeycodeAction.Num0, 0x1D },
            { KeycodeAction.Minus, 0x1B },
            { KeycodeAction.Equals, 0x18 },
            { KeycodeAction.LeftBracket, 0x21 },
            { KeycodeAction.RightBracket, 0x1E },
            { KeycodeAction.Backslash, 0x2A },
            { KeycodeAction.Semicolon, 0x29 },
            { KeycodeAction.Apostrophe, 0x27 },
            { KeycodeAction.Comma, 0x2B },
            { KeycodeAction.Period, 0x2F },
            { KeycodeAction.Slash, 0x2C },
            { KeycodeAction.Grave, 0x32 },
            { KeycodeAction.Tab, 0x30 },
            { KeycodeAction.Space, 0x31 },
            { KeycodeAction.Enter, 0x24 },
            { KeycodeAction.ForwardDel, 0x75 },
            { KeycodeAction.Del, 0x33 },
            { KeycodeAction.Back, 0x33 },
            { KeycodeAction.Escape, 0x35 },
            { KeycodeAction.CapsLock, 0x39 },
            { KeycodeAction.ShiftLeft, 0x38 },
            { KeycodeAction.ShiftRight, 0x3C },
            { KeycodeAction.CtrlLeft, 0x3B },
            { KeycodeAction.CtrlRight, 0x3E },
            { KeycodeAction.AltLeft, 0x3A },
            { KeycodeAction.AltRight, 0x3D },
            { KeycodeAction.MetaLeft, 0x37 },
            { KeycodeAction.MetaRight, 0x36 },
            { KeycodeAction.Function, 0x3F },
            { KeycodeAction.Home, 0x73 },
            { KeycodeAction.MoveEnd, 0x77 },
            { KeycodeAction.PageUp, 0x74 },
            { KeycodeAction.PageDown, 0x79 },
            { KeycodeAction.Insert, 0x72 },
            { KeycodeAction.DpadUp, 0x7E },
            { KeycodeAction.DpadDown, 0x7D },
            { KeycodeAction.DpadLeft, 0x7B },
            { KeycodeAction.DpadRight, 0x7C },
            { KeycodeAction.F1, 0x7A },
            { KeycodeAction.F2, 0x78 },
            { KeycodeAction.F3, 0x63 },
            { KeycodeAction.F4, 0x76 },
            { KeycodeAction.F5, 0x60 },
            { KeycodeAction.F6, 0x61 },
            { KeycodeAction.F7, 0x62 },
            { KeycodeAction.F8, 0x64 },
            { KeycodeAction.F9, 0x65 },
            { KeycodeAction.F10, 0x6D },
            { KeycodeAction.F11, 0x67 },
            { KeycodeAction.F12, 0x6F },
            { KeycodeAction.NumLock, 0x47 },
            { KeycodeAction.Numpad0, 0x52 },
            { KeycodeAction.Numpad1, 0x53 },
            { KeycodeAction.Numpad2, 0x54 },
            { KeycodeAction.Numpad3, 0x55 },
            { KeycodeAction.Numpad4, 0x56 },
            { KeycodeAction.Numpad5, 0x57 },
            { KeycodeAction.Numpad6, 0x58 },
            { KeycodeAction.Numpad7, 0x59 },
            { KeycodeAction.Numpad8, 0x5A },
            { KeycodeAction.Numpad9, 0x5B },
            { KeycodeAction.NumpadAdd, 0x45 },
            { KeycodeAction.NumpadSubtract, 0x4E },
            { KeycodeAction.NumpadDivide, 0x4B },
            { KeycodeAction.NumpadMultiply, 0x43 },
            { KeycodeAction.NumpadEnter, 0x4C },
            { KeycodeAction.NumpadDot, 0x41 },
            { KeycodeAction.NumpadEquals, 0x51 },
            { KeycodeAction.VolumeUp, 0x48 },
            { KeycodeAction.VolumeDown, 0x49 },
            { KeycodeAction.VolumeMute, 0x4A }
        };

        public void KeyPress(KeycodeAction keycodeAction)
        {
            if (!KeycodeMap.TryGetValue(keycodeAction, out ushort keycode)) return;

            PostKeyboardEvent(keycode, true);
            PostKeyboardEvent(keycode, false);
        }

        public void TextEntry(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            ushort[] unicodeCharacters = new ushort[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                unicodeCharacters[i] = text[i];
            }

            IntPtr keyDownEvent = CGEventCreateKeyboardEvent(IntPtr.Zero, 0, true);
            CGEventKeyboardSetUnicodeString(keyDownEvent, (ulong)unicodeCharacters.Length, unicodeCharacters);
            PostEvent(keyDownEvent);

            IntPtr keyUpEvent = CGEventCreateKeyboardEvent(IntPtr.Zero, 0, false);
            CGEventKeyboardSetUnicodeString(keyUpEvent, (ulong)unicodeCharacters.Length, unicodeCharacters);
            PostEvent(keyUpEvent);
        }

        private static void PostKeyboardEvent(ushort keycode, bool keyDown)
        {
            IntPtr keyboardEvent = CGEventCreateKeyboardEvent(IntPtr.Zero, keycode, keyDown);
            PostEvent(keyboardEvent);
        }

        private static void PostEvent(IntPtr eventRef)
        {
            if (eventRef == IntPtr.Zero) return;

            CGEventPost(CGEventTapLocation.Hid, eventRef);
            CFRelease(eventRef);
        }

        [DllImport(ApplicationServicesFramework)]
        private static extern IntPtr CGEventCreateKeyboardEvent(IntPtr source, ushort virtualKey, bool keyDown);

        [DllImport(ApplicationServicesFramework)]
        private static extern void CGEventKeyboardSetUnicodeString(IntPtr @event, ulong stringLength, ushort[] unicodeString);

        [DllImport(ApplicationServicesFramework)]
        private static extern void CGEventPost(CGEventTapLocation tap, IntPtr @event);

        [DllImport(ApplicationServicesFramework)]
        private static extern void CFRelease(IntPtr cfTypeRef);

        private enum CGEventTapLocation
        {
            Hid = 0
        }
    }
}
