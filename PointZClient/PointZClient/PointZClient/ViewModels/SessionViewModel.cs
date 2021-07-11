using System.Diagnostics;
using System.Windows.Input;
using PointZClient.Services.CommandSender;
using PointZClient.ViewModels.Base;
using Xamarin.Forms;

namespace PointZClient.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ICommandSenderService commandSenderService;

        public SessionViewModel(ICommandSenderService commandSenderService)
        {
            this.commandSenderService = commandSenderService;
            TouchpadTappedCommand = new Command(OnTouchpadTapped);
            TouchpadDragStartingCommand = new Command(OnTouchpadDragStarting);
        }

        public ICommand TouchpadTappedCommand { get; }
        public ICommand TouchpadDragStartingCommand { get; }

        private void OnTouchpadTapped(object data)
        {
            Debug.WriteLine($"Touchpad tapped, data: {data}");
        }
        private void OnTouchpadDragStarting(object data)
        {
            Debug.WriteLine($"Touchpad drag starting, data: {data}");
        }
    }
}