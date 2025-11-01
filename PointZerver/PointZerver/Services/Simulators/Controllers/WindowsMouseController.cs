using InputSimulatorStandard;

namespace PointZerver.Services.Simulators.Controllers
{
    public class WindowsMouseController : IMouseController
    {
        private readonly IMouseSimulator mouseSimulator;

        public WindowsMouseController(IMouseSimulator mouseSimulator) => this.mouseSimulator = mouseSimulator;

        public void HorizontalScroll(int amount) => this.mouseSimulator.HorizontalScroll(amount);

        public void VerticalScroll(int amount) => this.mouseSimulator.VerticalScroll(amount);

        public void LeftButtonClick() => this.mouseSimulator.LeftButtonClick();

        public void LeftButtonDown() => this.mouseSimulator.LeftButtonDown();

        public void LeftButtonUp() => this.mouseSimulator.LeftButtonUp();

        public void MiddleButtonClick() => this.mouseSimulator.MiddleButtonClick();

        public void MiddleButtonDown() => this.mouseSimulator.MiddleButtonDown();

        public void MiddleButtonUp() => this.mouseSimulator.MiddleButtonUp();

        public void RightButtonClick() => this.mouseSimulator.RightButtonClick();

        public void RightButtonDown() => this.mouseSimulator.RightButtonDown();

        public void RightButtonUp() => this.mouseSimulator.RightButtonUp();

        public void MoveMouseBy(int x, int y) => this.mouseSimulator.MoveMouseBy(x, y);

        public void MoveMouseTo(double x, double y) => this.mouseSimulator.MoveMouseTo(x, y);

        public void MoveMouseToPositionOnVirtualDesktop(double x, double y) =>
            this.mouseSimulator.MoveMouseToPositionOnVirtualDesktop(x, y);
    }
}
