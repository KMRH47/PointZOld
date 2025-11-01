using System;
using System.Collections.Generic;
using SharpHook.Native;

namespace PointZerver.Services.VirtualKeyCodeMapper
{
    public class VirtualKeyCodeMapperService : IVirtualKeyCodeMapperService
    {
        private static readonly IReadOnlyDictionary<KeycodeAction, KeyCode> KeyCodeMap =
            new Dictionary<KeycodeAction, KeyCode>
            {
                { KeycodeAction.A, KeyCode.VcA },
                { KeycodeAction.B, KeyCode.VcB },
                { KeycodeAction.C, KeyCode.VcC },
                { KeycodeAction.D, KeyCode.VcD },
                { KeycodeAction.E, KeyCode.VcE },
                { KeycodeAction.F, KeyCode.VcF },
                { KeycodeAction.G, KeyCode.VcG },
                { KeycodeAction.H, KeyCode.VcH },
                { KeycodeAction.I, KeyCode.VcI },
                { KeycodeAction.J, KeyCode.VcJ },
                { KeycodeAction.K, KeyCode.VcK },
                { KeycodeAction.L, KeyCode.VcL },
                { KeycodeAction.M, KeyCode.VcM },
                { KeycodeAction.N, KeyCode.VcN },
                { KeycodeAction.O, KeyCode.VcO },
                { KeycodeAction.P, KeyCode.VcP },
                { KeycodeAction.Q, KeyCode.VcQ },
                { KeycodeAction.R, KeyCode.VcR },
                { KeycodeAction.S, KeyCode.VcS },
                { KeycodeAction.T, KeyCode.VcT },
                { KeycodeAction.U, KeyCode.VcU },
                { KeycodeAction.V, KeyCode.VcV },
                { KeycodeAction.W, KeyCode.VcW },
                { KeycodeAction.X, KeyCode.VcX },
                { KeycodeAction.Y, KeyCode.VcY },
                { KeycodeAction.Z, KeyCode.VcZ },
                { KeycodeAction.Num0, KeyCode.Vc0 },
                { KeycodeAction.Num1, KeyCode.Vc1 },
                { KeycodeAction.Num2, KeyCode.Vc2 },
                { KeycodeAction.Num3, KeyCode.Vc3 },
                { KeycodeAction.Num4, KeyCode.Vc4 },
                { KeycodeAction.Num5, KeyCode.Vc5 },
                { KeycodeAction.Num6, KeyCode.Vc6 },
                { KeycodeAction.Num7, KeyCode.Vc7 },
                { KeycodeAction.Num8, KeyCode.Vc8 },
                { KeycodeAction.Num9, KeyCode.Vc9 },
                { KeycodeAction.Minus, KeyCode.VcMinus },
                { KeycodeAction.Plus, KeyCode.VcEquals },
                { KeycodeAction.Equals, KeyCode.VcEquals },
                { KeycodeAction.Comma, KeyCode.VcComma },
                { KeycodeAction.Period, KeyCode.VcPeriod },
                { KeycodeAction.Slash, KeyCode.VcSlash },
                { KeycodeAction.Backslash, KeyCode.VcBackslash },
                { KeycodeAction.Semicolon, KeyCode.VcSemicolon },
                { KeycodeAction.Apostrophe, KeyCode.VcQuote },
                { KeycodeAction.Grave, KeyCode.VcGrave },
                { KeycodeAction.LeftBracket, KeyCode.VcOpenBracket },
                { KeycodeAction.RightBracket, KeyCode.VcCloseBracket },
                { KeycodeAction.Space, KeyCode.VcSpace },
                { KeycodeAction.Tab, KeyCode.VcTab },
                { KeycodeAction.Enter, KeyCode.VcEnter },
                { KeycodeAction.NumpadEnter, KeyCode.VcEnter },
                { KeycodeAction.Back, KeyCode.VcBackspace },
                { KeycodeAction.Del, KeyCode.VcBackspace },
                { KeycodeAction.ForwardDel, KeyCode.VcDelete },
                { KeycodeAction.Clear, KeyCode.VcDelete },
                { KeycodeAction.Home, KeyCode.VcHome },
                { KeycodeAction.MoveHome, KeyCode.VcHome },
                { KeycodeAction.MoveEnd, KeyCode.VcEnd },
                { KeycodeAction.PageUp, KeyCode.VcPageUp },
                { KeycodeAction.PageDown, KeyCode.VcPageDown },
                { KeycodeAction.DpadUp, KeyCode.VcUp },
                { KeycodeAction.DpadDown, KeyCode.VcDown },
                { KeycodeAction.DpadLeft, KeyCode.VcLeft },
                { KeycodeAction.DpadRight, KeyCode.VcRight },
                { KeycodeAction.CapsLock, KeyCode.VcCapsLock },
                { KeycodeAction.ShiftLeft, KeyCode.VcLeftShift },
                { KeycodeAction.ShiftRight, KeyCode.VcRightShift },
                { KeycodeAction.CtrlLeft, KeyCode.VcLeftControl },
                { KeycodeAction.CtrlRight, KeyCode.VcRightControl },
                { KeycodeAction.AltLeft, KeyCode.VcLeftAlt },
                { KeycodeAction.AltRight, KeyCode.VcRightAlt },
                { KeycodeAction.MetaLeft, KeyCode.VcLeftMeta },
                { KeycodeAction.MetaRight, KeyCode.VcRightMeta },
                { KeycodeAction.Menu, KeyCode.VcMenu },
                { KeycodeAction.Escape, KeyCode.VcEscape },
                { KeycodeAction.Insert, KeyCode.VcInsert },
                { KeycodeAction.Break, KeyCode.VcPause },
                { KeycodeAction.ScrollLock, KeyCode.VcScrollLock },
                { KeycodeAction.NumLock, KeyCode.VcNumLock },
                { KeycodeAction.Numpad0, KeyCode.VcNumpad0 },
                { KeycodeAction.Numpad1, KeyCode.VcNumpad1 },
                { KeycodeAction.Numpad2, KeyCode.VcNumpad2 },
                { KeycodeAction.Numpad3, KeyCode.VcNumpad3 },
                { KeycodeAction.Numpad4, KeyCode.VcNumpad4 },
                { KeycodeAction.Numpad5, KeyCode.VcNumpad5 },
                { KeycodeAction.Numpad6, KeyCode.VcNumpad6 },
                { KeycodeAction.Numpad7, KeyCode.VcNumpad7 },
                { KeycodeAction.Numpad8, KeyCode.VcNumpad8 },
                { KeycodeAction.Numpad9, KeyCode.VcNumpad9 },
                { KeycodeAction.NumpadAdd, KeyCode.VcNumpadAdd },
                { KeycodeAction.NumpadSubtract, KeyCode.VcNumpadSubtract },
                { KeycodeAction.NumpadMultiply, KeyCode.VcNumpadMultiply },
                { KeycodeAction.NumpadDivide, KeyCode.VcNumpadDivide },
                { KeycodeAction.NumpadDot, KeyCode.VcNumpadDecimal },
                { KeycodeAction.NumpadComma, KeyCode.VcNumpadDecimal },
                { KeycodeAction.NumpadEquals, KeyCode.VcNumpadEquals },
                { KeycodeAction.F1, KeyCode.VcF1 },
                { KeycodeAction.F2, KeyCode.VcF2 },
                { KeycodeAction.F3, KeyCode.VcF3 },
                { KeycodeAction.F4, KeyCode.VcF4 },
                { KeycodeAction.F5, KeyCode.VcF5 },
                { KeycodeAction.F6, KeyCode.VcF6 },
                { KeycodeAction.F7, KeyCode.VcF7 },
                { KeycodeAction.F8, KeyCode.VcF8 },
                { KeycodeAction.F9, KeyCode.VcF9 },
                { KeycodeAction.F10, KeyCode.VcF10 },
                { KeycodeAction.F11, KeyCode.VcF11 },
                { KeycodeAction.F12, KeyCode.VcF12 },
                { KeycodeAction.VolumeMute, KeyCode.VcVolumeMute },
                { KeycodeAction.VolumeDown, KeyCode.VcVolumeDown },
                { KeycodeAction.VolumeUp, KeyCode.VcVolumeUp },
                { KeycodeAction.MediaPlay, KeyCode.VcMediaPlay },
                { KeycodeAction.MediaPlayPause, KeyCode.VcMediaPlay },
                { KeycodeAction.MediaStop, KeyCode.VcMediaStop },
                { KeycodeAction.MediaNext, KeyCode.VcMediaNext },
                { KeycodeAction.MediaPrevious, KeyCode.VcMediaPrevious },
                { KeycodeAction.MediaFastForward, KeyCode.VcMediaNext },
                { KeycodeAction.MediaRewind, KeyCode.VcMediaPrevious },
                { KeycodeAction.MediaPause, KeyCode.VcMediaPlay },
                { KeycodeAction.Forward, KeyCode.VcBrowserForward }
            };

        public KeyCode ParseString(string keycodeString)
        {
            bool keyCodeInvalid = !Enum.TryParse(keycodeString, out KeycodeAction keyCodeAction);

            if (keyCodeInvalid)
            {
                return KeyCode.VcUndefined;
            }

            return KeyCodeMap.TryGetValue(keyCodeAction, out KeyCode keyCode)
                ? keyCode
                : KeyCode.VcUndefined;
        }
    }
}
