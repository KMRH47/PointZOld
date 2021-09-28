using System;
using System.Diagnostics;
using Android.Views;
using PointZ.Models.PlatformEvent;
using PointZ.Services.PlatformEvent;

namespace PointZ.Android.Services.KeyEventHandler
{
    public class KeyEventHandlerService : IKeyEventHandlerService
    {
        private readonly ISessionPlatformEventService sessionPlatformEventService;

        public KeyEventHandlerService(ISessionPlatformEventService sessionPlatformEventService)
        {
            this.sessionPlatformEventService = sessionPlatformEventService;
        }

        public void Handle(KeyEvent e)
        {
            KeyAction keyAction = (KeyAction)((ushort)e.Action);

            switch (e.Action)
            {
                case KeyEventActions.Down:
                case KeyEventActions.Multiple:

                    if (e.KeyCode != Keycode.Unknown)
                    {
                        if (e.UnicodeChar == 0)
                        {
                            this.sessionPlatformEventService.NotifyOnKeyDown(keyAction, e.KeyCode.ToString());
                        }
                        else
                        {
                            char symbol = (char)e.UnicodeChar;
                            this.sessionPlatformEventService.NotifyOnKeyDown(keyAction, symbol.ToString()); 
                        }
                    }
                    else
                    {
                        if (e.Characters.Length > 0)
                        {
                            this.sessionPlatformEventService.NotifyOnKeyDown(keyAction, e.Characters);
                        }
                    }

                    break;
                case KeyEventActions.Up:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}