using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using PointZerver.Extensions;
using PointZerver.Services.CommandConverter;
using PointZerver.Services.Logger;

namespace PointZerver.Services.Simulators
{
    public class KeyboardSimulatorService : IInputSimulatorService
    {
        private readonly IKeyboardSimulator keyboardSimulator;
        private readonly IVirtualKeyCodeConverterService virtualKeyCodeConverterService;

        public KeyboardSimulatorService(
            IKeyboardSimulator keyboardSimulator, IVirtualKeyCodeConverterService virtualKeyCodeConverterService)
        {
            this.keyboardSimulator = keyboardSimulator;
            this.virtualKeyCodeConverterService = virtualKeyCodeConverterService;
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
                        this.keyboardSimulator.TextEntry(payload);
                    }
                    else
                    {
                        this.keyboardSimulator.KeyPress(this.virtualKeyCodeConverterService.ParseString(payload));
                    }
                    break;
                case "KeyPress":
                    this.keyboardSimulator.KeyPress(this.virtualKeyCodeConverterService.ParseString(payload));
                    break;
                case "TextEntry":
                    this.keyboardSimulator.TextEntry(payload);
                    break;
                case "ModifiedKeyStroke":
                    //VirtualKeyCode[] parameters = await GetParams(data);
                    //this.keyboardSimulator.KeyPress(parameters);
                    // this.keyboardSimulator.ModifiedKeyStroke();
                    break;
            }

            return Task.CompletedTask;
        }

        private Task<VirtualKeyCode[]> GetParams(IReadOnlyList<string> data)
        {
            List<VirtualKeyCode> dataList = new();

            for (int i = 2; i < data.Count; i++)
            {
                string arg = data[i];
                bool parseUnsuccessful = !Enum.TryParse(arg, out VirtualKeyCode key);
                if (parseUnsuccessful) throw ArgumentException(arg);
                dataList.Add(key);
            }

            return Task.FromResult(dataList.ToArray());
        }

        private static ArgumentException ArgumentException(object contextData) =>
            new($"No VirtualKeyCode matching {contextData}.");
    }
}