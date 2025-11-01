using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointZerver.Extensions;
using PointZerver.Services.VirtualKeyCodeMapper;
using SharpHook;
using SharpHook.Native;

namespace PointZerver.Services.Simulators
{
    public class KeyboardSimulatorService : IInputSimulatorService
    {
        private readonly IEventSimulator eventSimulator;
        private readonly IVirtualKeyCodeMapperService virtualKeyCodeMapperService;

        public KeyboardSimulatorService(
            IEventSimulator eventSimulator, IVirtualKeyCodeMapperService virtualKeyCodeMapperService)
        {
            this.eventSimulator = eventSimulator;
            this.virtualKeyCodeMapperService = virtualKeyCodeMapperService;
        }

        public string CommandId => "K";

        public Task ExecuteCommand(string data)
        {
            string command = data.TakeFromToNext(',', 2);
            int headerLength = command.Length + 3;
            string payload = data.TakeFrom(headerLength);

            switch (command)
            {
                case "KeyDown":
                    if (payload.Length == 1)
                    {
                        this.eventSimulator.SimulateTextTyping(payload);
                    }
                    else
                    {
                        SimulateKeyStroke(payload);
                    }
                    break;
                case "KeyPress":
                    SimulateKeyStroke(payload);
                    break;
                case "TextEntry":
                    this.eventSimulator.SimulateTextTyping(payload);
                    break;
                case "ModifiedKeyStroke":
                    break;
            }

            return Task.CompletedTask;
        }

        private void SimulateKeyStroke(string payload)
        {
            string[] parts = payload.Split('+', StringSplitOptions.RemoveEmptyEntries);
            List<KeyCode> keyCodes = parts.Select(this.virtualKeyCodeMapperService.ParseString)
                .Where(code => code != KeyCode.VcUndefined)
                .ToList();

            if (keyCodes.Count == 0)
            {
                return;
            }

            foreach (KeyCode keyCode in keyCodes)
            {
                this.eventSimulator.SimulateKeyPress(keyCode);
            }

            for (int i = keyCodes.Count - 1; i >= 0; i--)
            {
                this.eventSimulator.SimulateKeyRelease(keyCodes[i]);
            }
        }
    }
}
