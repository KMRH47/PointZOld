using System.Diagnostics;
using System.Threading.Tasks;
using InputSimulatorStandard;

namespace PointZerver.Services.Simulators
{
    public class MouseSimulatorService : IInputSimulatorService
    {
        private readonly IMouseSimulator mouseSimulator;

        public MouseSimulatorService(IMouseSimulator mouseSimulator) => this.mouseSimulator = mouseSimulator;

        public string CommandId => "M";

        public Task ExecuteCommand(string data)
        {
            string[] dataSplit = data.Split(',');

            switch (dataSplit[1])
            {
                case "HorizontalScroll":
                    this.mouseSimulator.HorizontalScroll(int.Parse(dataSplit[2]));
                    break;
                case "VerticalScroll":
                    Debug.WriteLine($"Vertical SCroll! amount:{dataSplit[2]}");
                    this.mouseSimulator.VerticalScroll(int.Parse(dataSplit[2]));
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
                    Debug.WriteLine($"Moving mouse by {dataSplit[2]}, {dataSplit[3]}");
                    this.mouseSimulator.MoveMouseBy(int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
                    break;
                case "MoveMouseTo":
                    // await this.logger.Log($"x: {dataSplit[2]} y: {dataSplit[3]}", this);
                    this.mouseSimulator.MoveMouseTo(double.Parse(dataSplit[2]), double.Parse(dataSplit[3]));
                    break;
                case "MoveMouseToPositionOnVirtualDesktop":
                    this.mouseSimulator.MoveMouseToPositionOnVirtualDesktop(double.Parse(dataSplit[2]),
                        double.Parse(dataSplit[3]));
                    break;
            }
            
            return Task.CompletedTask;
        }
    }
}