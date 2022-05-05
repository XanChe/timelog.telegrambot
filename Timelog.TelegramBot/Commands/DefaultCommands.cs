using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Commands
{
    public class DefaultCommands
    {
        [CommandBind("/start")]
        public async Task StartCommand(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, "Добро пожаловать!");
        }
    }
}
