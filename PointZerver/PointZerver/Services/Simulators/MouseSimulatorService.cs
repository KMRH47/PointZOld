using System.Threading.Tasks;
using InputSimulatorStandard;
using PointZ.Services.Logger;

namespace PointZ.Services.Simulators
{
    public class MouseSimulatorService : BaseInputSimulator
    {
        private readonly IMouseSimulator mouseSimulator;

        public MouseSimulatorService(IMouseSimulator mouseSimulator, ILogger logger) : base(logger) =>
            this.mouseSimulator = mouseSimulator;

        public override string CommandId => "C";

        public override Task ExecuteCommand(string[] data)
        {
            string command = data[1];

            switch (command)
            {
                case "HS":
                    this.mouseSimulator.HorizontalScroll(int.Parse(data[2]));
                    break;
                case "VS":
                    this.mouseSimulator.VerticalScroll(int.Parse(data[2]));
                    break;
                case "LBC":
                    this.mouseSimulator.LeftButtonClick();
                    break;
                case "LBD":
                    this.mouseSimulator.LeftButtonDown();
                    break;
                case "LBU":
                    this.mouseSimulator.LeftButtonUp();
                    break;
                case "MBC":
                    this.mouseSimulator.MiddleButtonClick();
                    break;
                case "MBD":
                    this.mouseSimulator.MiddleButtonDown();
                    break;
                case "MBU":
                    this.mouseSimulator.MiddleButtonUp();
                    break;
                case "RBC":
                    this.mouseSimulator.RightButtonClick();
                    break;
                case "RBD":
                    this.mouseSimulator.RightButtonDown();
                    break;
                case "RBU":
                    this.mouseSimulator.RightButtonUp();
                    break;
                case "MB":
                    this.mouseSimulator.MoveMouseBy(int.Parse(data[2]), int.Parse(data[3]));
                    break;
                case "MT":
                    base.Logger.Log($"x: {data[2]} y: {data[3]}", this);
                    this.mouseSimulator.MoveMouseTo(int.Parse(data[2]), int.Parse(data[3]));
                    break;
                case "M":
                    this.mouseSimulator.MoveMouseToPositionOnVirtualDesktop(int.Parse(data[2]), int.Parse(data[3]));
                    break;
            }

            return Task.CompletedTask;
        }
    }
}