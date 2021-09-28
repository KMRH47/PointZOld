using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using PointZerver.Services.CommandConverter;
using PointZerver.Services.Logger;

namespace PointZerver.Services.Simulators
{
    public class KeyboardSimulatorService : BaseInputSimulator
    {
        private readonly IKeyboardSimulator keyboardSimulator;
        private readonly IVirtualKeyCodeConverterService virtualKeyCodeConverterService;

        private bool shift;

        public KeyboardSimulatorService(IKeyboardSimulator keyboardSimulator,
            IVirtualKeyCodeConverterService virtualKeyCodeConverterService, ILogger logger) : base(logger)
        {
            this.keyboardSimulator = keyboardSimulator;
            this.virtualKeyCodeConverterService = virtualKeyCodeConverterService;
        }

        public override string CommandId => "K";

        public override async Task ExecuteCommand(string[] data)
        {
            string command = data[1];

            switch (command)
            {
                case "KeyDown":
                    string keyCode = data[2];

                    if (keyCode.Length == 1)
                    {
                        this.keyboardSimulator.TextEntry(keyCode);
                    }
                    else
                    {
                        VirtualKeyCode virtualKeyCode = this.virtualKeyCodeConverterService.ParseString(keyCode);

                        this.keyboardSimulator.KeyPress(virtualKeyCode);
                    }

                    break;
                case "KeyPress":
                    break;
                case "TextEntry":
                    string message = data[2];
                    this.keyboardSimulator.TextEntry(message);
                    break;
                case "ModifiedKeyStroke":
                    VirtualKeyCode[] parameters = await GetParams(data);
                    this.keyboardSimulator.KeyPress(parameters);
                    // this.keyboardSimulator.ModifiedKeyStroke();
                    break;
            }
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