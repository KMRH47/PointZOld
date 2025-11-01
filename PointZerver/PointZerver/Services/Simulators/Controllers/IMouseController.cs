namespace PointZerver.Services.Simulators.Controllers
{
    public interface IMouseController
    {
        void HorizontalScroll(int amount);
        void VerticalScroll(int amount);
        void LeftButtonClick();
        void LeftButtonDown();
        void LeftButtonUp();
        void MiddleButtonClick();
        void MiddleButtonDown();
        void MiddleButtonUp();
        void RightButtonClick();
        void RightButtonDown();
        void RightButtonUp();
        void MoveMouseBy(int x, int y);
        void MoveMouseTo(double x, double y);
        void MoveMouseToPositionOnVirtualDesktop(double x, double y);
    }
}
