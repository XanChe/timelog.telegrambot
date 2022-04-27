using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Timelog.TelegramBot.Commands
{
    public class ChatCommands
    {
        [CommandBind("/bind_project")]
        public async Task BindProjectToCurrentChatCommand(ITelegramBotClient botClient, Update update, string parameters)
        {
            throw new NotImplementedException();
        }
    }
}
