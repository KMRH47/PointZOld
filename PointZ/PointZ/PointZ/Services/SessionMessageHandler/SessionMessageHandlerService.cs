using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Services.CommandSender;
using PointZ.Services.CommandSender.Base;

namespace PointZ.Services.SessionMessageHandler
{
    public class SessionMessageHandlerService : ISessionMessageHandlerService
    {
        
        private readonly ICommandSender commandSender;

        public SessionMessageHandlerService(ICommandSender commandSender)
        {
            this.commandSender = commandSender;
        }
        
        public async Task SendMessageAsync(string message)
        {
            await this.commandSender.SendAsync(KeyboardCommand.TextEntry, message);
        }
    }
}