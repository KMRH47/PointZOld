using System;
using System.Net;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;
using PointZ.Services.CommandSender;

namespace PointZ.Services.SessionEventHandler
{
    public class SessionKeyEventHandlerService : ISessionEventHandlerService<KeyEventArgs>
    {
        private readonly ICommandSenderService commandSenderService;

        public SessionKeyEventHandlerService(ICommandSenderService commandSenderService)
        {
            this.commandSenderService = commandSenderService;
        }

        public void Bind(IPAddress ipAddress) => this.commandSenderService.Bind(ipAddress);

      

        public async Task HandleAsync(KeyEventArgs e)
        {
            switch (e.KeyAction)
            {
                case KeyAction.Up:
                    break;
                case KeyAction.Multiple:
                case KeyAction.Down:
                    string data = $"{e.KeyCode}";

                    await this.commandSenderService.SendAsync(KeyboardCommand.KeyDown, data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}