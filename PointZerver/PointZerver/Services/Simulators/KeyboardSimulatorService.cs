using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using PointZerver.Services.Logger;

namespace PointZerver.Services.Simulators
{
    public class KeyboardSimulatorService : BaseInputSimulator
    {
        private readonly IKeyboardSimulator keyboardSimulator;

        public KeyboardSimulatorService(IKeyboardSimulator keyboardSimulator, ILogger logger) : base(logger) =>
            this.keyboardSimulator = keyboardSimulator;

        public override string CommandId => "K";

        public override async Task ExecuteCommand(string[] data)
        {
            string command = data[1];

            switch (command)
            {
                case "KD":
                    string arg = data[2];
                    bool parseUnsuccessful = !Enum.TryParse(arg, out VirtualKeyCode key);
                    if (parseUnsuccessful) throw ArgumentException(arg);
                    this.keyboardSimulator.KeyDown(key);
                    break;
                case "KP":
                    VirtualKeyCode[] parameters = await GetParams(data);
                    this.keyboardSimulator.KeyPress(parameters);
                    break;
                case "TE":
                    //this.keyboardSimulator.TextEntry();
                    break;
                case "MKS":
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