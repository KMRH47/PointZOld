using System.Diagnostics;
using System.Threading.Tasks;
using SharpHook;
using SharpHook.Native;

namespace PointZerver.Services.Simulators
{
    public class MouseSimulatorService : IInputSimulatorService
    {
        private readonly IEventSimulator eventSimulator;

        public MouseSimulatorService(IEventSimulator eventSimulator) => this.eventSimulator = eventSimulator;

        public string CommandId => "M";

        public Task ExecuteCommand(string data)
        {
            string[] dataSplit = data.Split(',');

            switch (dataSplit[1])
            {
                case "HorizontalScroll":
                    this.eventSimulator.SimulateMouseWheel(UioHookWheelDirection.Horizontal,
                        ParseScrollAmount(dataSplit[2]));
                    break;
                case "VerticalScroll":
                    Debug.WriteLine($"Vertical Scroll! amount:{dataSplit[2]}");
                    this.eventSimulator.SimulateMouseWheel(UioHookWheelDirection.Vertical,
                        ParseScrollAmount(dataSplit[2]));
                    break;
                case "LeftButtonClick":
                    SimulateClick(MouseButton.Button1);
                    break;
                case "LeftButtonDown":
                    this.eventSimulator.SimulateMousePress(MouseButton.Button1);
                    break;
                case "LeftButtonUp":
                    this.eventSimulator.SimulateMouseRelease(MouseButton.Button1);
                    break;
                case "MiddleButtonClick":
                    SimulateClick(MouseButton.Button3);
                    break;
                case "MiddleButtonDown":
                    this.eventSimulator.SimulateMousePress(MouseButton.Button3);
                    break;
                case "MiddleButtonUp":
                    this.eventSimulator.SimulateMouseRelease(MouseButton.Button3);
                    break;
                case "RightButtonClick":
                    SimulateClick(MouseButton.Button2);
                    break;
                case "RightButtonDown":
                    this.eventSimulator.SimulateMousePress(MouseButton.Button2);
                    break;
                case "RightButtonUp":
                    this.eventSimulator.SimulateMouseRelease(MouseButton.Button2);
                    break;
                case "MoveMouseBy":
                    Debug.WriteLine($"Moving mouse by {dataSplit[2]}, {dataSplit[3]}");
                    this.eventSimulator.SimulateMouseMovementRelative(ParseMovement(dataSplit[2]),
                        ParseMovement(dataSplit[3]));
                    break;
                case "MoveMouseTo":
                    this.eventSimulator.SimulateMouseMovement(new Point(ParseCoordinate(dataSplit[2]),
                        ParseCoordinate(dataSplit[3])));
                    break;
                case "MoveMouseToPositionOnVirtualDesktop":
                    this.eventSimulator.SimulateMouseMovement(new Point(ParseCoordinate(dataSplit[2]),
                        ParseCoordinate(dataSplit[3])));
                    break;
            }

            return Task.CompletedTask;
        }

        private void SimulateClick(MouseButton button)
        {
            this.eventSimulator.SimulateMousePress(button);
            this.eventSimulator.SimulateMouseRelease(button);
        }

        private static short ParseScrollAmount(string value) => (short)int.Parse(value);

        private static short ParseMovement(string value) => (short)int.Parse(value);

        private static int ParseCoordinate(string value) => int.Parse(value);
    }
}
