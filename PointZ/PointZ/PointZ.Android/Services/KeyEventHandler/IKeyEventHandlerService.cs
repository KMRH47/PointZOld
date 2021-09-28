using Android.Views;

namespace PointZ.Android.Services.KeyEventHandler
{
    public interface IKeyEventHandlerService
    {
        void Handle(KeyEvent e);
    }
}