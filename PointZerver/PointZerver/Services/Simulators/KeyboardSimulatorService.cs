using System;
using System.Threading.Tasks;
using PointZerver.Extensions;
using PointZerver.Services.Simulators.Controllers;
using PointZerver.Services.VirtualKeyCodeMapper;

namespace PointZerver.Services.Simulators
{
    public class KeyboardSimulatorService : IInputSimulatorService
    {
        private readonly IKeyboardController keyboardController;

        public KeyboardSimulatorService(IKeyboardController keyboardController) =>
            this.keyboardController = keyboardController;

        public string CommandId => "K";

        public Task ExecuteCommand(string data)
        {
            string command = data.TakeFromToNext(',', 2);
            int headerLength = command.Length + 3;
            string payload = data.TakeFrom(headerLength);

            switch (command)
            {
                case "KeyDown":
                    HandleKeyAction(payload);
                    break;
                case "KeyPress":
                    HandleKeyAction(payload);
                    break;
                case "TextEntry":
                    this.keyboardController.TextEntry(payload);
                    break;
                case "ModifiedKeyStroke":
                    break;
            }

            return Task.CompletedTask;
        }

        private void HandleKeyAction(string payload)
        {
            if (string.IsNullOrEmpty(payload)) return;

            if (payload.Length == 1)
            {
                this.keyboardController.TextEntry(payload);
                return;
            }

            bool parsed = Enum.TryParse(payload, out KeycodeAction keycodeAction);
            if (!parsed) return;

            this.keyboardController.KeyPress(keycodeAction);
        }
    }
}
