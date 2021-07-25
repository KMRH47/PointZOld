using System.Diagnostics;
using PointZClient.Services.CommandSender;
using PointZClient.Services.TouchEventService;
using PointZClient.ViewModels.Base;

namespace PointZClient.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ICommandSenderService commandSenderService;

        public SessionViewModel(ICommandSenderService commandSenderService, ITouchEventService touchEventService)
        {
            this.commandSenderService = commandSenderService;
            touchEventService.ScreenTouched+= OnScreenTouched;
        }

        private static void OnScreenTouched(object sender, TouchEventArgs e)
        {
            Debug.WriteLine($"Touch at X: {e.X} Y: {e.Y}");
        }
    }
}