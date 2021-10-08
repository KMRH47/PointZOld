using System;
using System.Net;
using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;
using PointZ.Services.CommandSender;
using PointZ.Services.CommandSender.Base;

namespace PointZ.Services.SessionEventHandler
{
    public class SessionKeyEventHandlerService : ISessionEventHandlerService<KeyEventArgs>
    {
        private readonly ICommandSender commandSender;

        public SessionKeyEventHandlerService(ICommandSender commandSender)
        {
            this.commandSender = commandSender;
        }

        public void Bind(IPEndPoint ipEndPoint) => this.commandSender.Bind(ipEndPoint);

      

        public async Task HandleAsync(KeyEventArgs e)
        {
            switch (e.KeyAction)
            {
                case KeyAction.Up:
                    break;
                case KeyAction.Multiple:
                case KeyAction.Down:
                    string data = $"{e.KeyCode}";

                    await this.commandSender.SendAsync(KeyboardCommand.KeyDown, data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}