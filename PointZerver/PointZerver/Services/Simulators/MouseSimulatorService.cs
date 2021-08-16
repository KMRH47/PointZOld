using System.Diagnostics;
using System.Threading.Tasks;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using PointZerver.Services.Logger;

namespace PointZerver.Services.Simulators
{
    public class MouseSimulatorService : BaseInputSimulator
    {
        private readonly IMouseSimulator mouseSimulator;

        public MouseSimulatorService(IMouseSimulator mouseSimulator, ILogger logger) : base(logger) =>
            this.mouseSimulator = mouseSimulator;

        public override string CommandId => "M";

        public override Task ExecuteCommand(string[] data)
        {
            string command = data[1];
            
            switch (command)
            {
                case "HorizontalScroll":
                    this.mouseSimulator.HorizontalScroll(int.Parse(data[2]));
                    break;
                case "VerticalScroll":
                    this.mouseSimulator.VerticalScroll(int.Parse(data[2]));
                    break;
                case "LeftButtonClick":
                    this.mouseSimulator.LeftButtonClick();
                    break;
                case "LeftButtonDown":
                    this.mouseSimulator.LeftButtonDown();
                    break;
                case "LeftButtonUp":
                    this.mouseSimulator.LeftButtonUp();
                    break;
                case "MiddleButtonClick":
                    this.mouseSimulator.MiddleButtonClick();
                    break;
                case "MiddleButtonDown":
                    this.mouseSimulator.MiddleButtonDown();
                    break;
                case "MiddleButtonUp":
                    this.mouseSimulator.MiddleButtonUp();
                    break;
                case "RightButtonClick":
                    this.mouseSimulator.RightButtonClick();
                    break;
                case "RightButtonDown":
                    this.mouseSimulator.RightButtonDown();
                    break;
                case "RightButtonUp":
                    this.mouseSimulator.RightButtonUp();
                    break;
                case "MoveMouseBy":
                    Debug.WriteLine($"Moving mouse by {data[2]}, {data[3]}");
                    this.mouseSimulator.MoveMouseBy(int.Parse(data[2]), int.Parse(data[3]));
                    break;
                case "MoveMouseTo":
                    base.Logger.Log($"x: {data[2]} y: {data[3]}", this);
                    this.mouseSimulator.MoveMouseTo(double.Parse(data[2]), double.Parse(data[3]));
                    break;
                case "MoveMouseToPositionOnVirtualDesktop":
                    this.mouseSimulator.MoveMouseToPositionOnVirtualDesktop(double.Parse(data[2]), double.Parse(data[3]));
                    break;
            }

            return Task.CompletedTask;
        }
    }
}