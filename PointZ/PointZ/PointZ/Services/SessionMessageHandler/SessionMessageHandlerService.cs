using System.Threading.Tasks;
using PointZ.Models.Command;
using PointZ.Services.CommandSender;

namespace PointZ.Services.SessionMessageHandler
{
    public class SessionMessageHandlerService : ISessionMessageHandlerService
    {
        
        private readonly ICommandSenderService commandSenderService;

        public SessionMessageHandlerService(ICommandSenderService commandSenderService)
        {
            this.commandSenderService = commandSenderService;
        }
        
        public async Task SendMessageAsync(string message)
        {
            await this.commandSenderService.SendAsync(KeyboardCommand.TextEntry, message);
        }
    }
}