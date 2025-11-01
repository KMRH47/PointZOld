using System.Diagnostics;
using System.Threading.Tasks;
using PointZerver.Services.Simulators.Controllers;

namespace PointZerver.Services.Simulators
{
    public class MouseSimulatorService : IInputSimulatorService
    {
        private readonly IMouseController mouseController;

        public MouseSimulatorService(IMouseController mouseController) => this.mouseController = mouseController;

        public string CommandId => "M";

        public Task ExecuteCommand(string data)
        {
            string[] dataSplit = data.Split(',');

            switch (dataSplit[1])
            {
                case "HorizontalScroll":
                    this.mouseController.HorizontalScroll(int.Parse(dataSplit[2]));
                    break;
                case "VerticalScroll":
                    Debug.WriteLine($"Vertical Scroll amount:{dataSplit[2]}");
                    this.mouseController.VerticalScroll(int.Parse(dataSplit[2]));
                    break;
                case "LeftButtonClick":
                    this.mouseController.LeftButtonClick();
                    break;
                case "LeftButtonDown":
                    this.mouseController.LeftButtonDown();
                    break;
                case "LeftButtonUp":
                    this.mouseController.LeftButtonUp();
                    break;
                case "MiddleButtonClick":
                    this.mouseController.MiddleButtonClick();
                    break;
                case "MiddleButtonDown":
                    this.mouseController.MiddleButtonDown();
                    break;
                case "MiddleButtonUp":
                    this.mouseController.MiddleButtonUp();
                    break;
                case "RightButtonClick":
                    this.mouseController.RightButtonClick();
                    break;
                case "RightButtonDown":
                    this.mouseController.RightButtonDown();
                    break;
                case "RightButtonUp":
                    this.mouseController.RightButtonUp();
                    break;
                case "MoveMouseBy":
                    Debug.WriteLine($"Moving mouse by {dataSplit[2]}, {dataSplit[3]}");
                    this.mouseController.MoveMouseBy(int.Parse(dataSplit[2]), int.Parse(dataSplit[3]));
                    break;
                case "MoveMouseTo":
                    this.mouseController.MoveMouseTo(double.Parse(dataSplit[2]), double.Parse(dataSplit[3]));
                    break;
                case "MoveMouseToPositionOnVirtualDesktop":
                    this.mouseController.MoveMouseToPositionOnVirtualDesktop(double.Parse(dataSplit[2]),
                        double.Parse(dataSplit[3]));
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
