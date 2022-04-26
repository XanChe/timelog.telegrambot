using Telegram.Bot;
using Telegram.Bot.Types;
using Timelog.TelegramBot.Models;

namespace Timelog.TelegramBot.Interfaces
{    
    public interface IBotCommandsService
    {
        public void RegisterCommand(string botCommant, Command command);
        public void UnregisterCommand(string botCommant);
        public Command? GetCommand(string botCommant);
        //public Task ExecuteCommandAsync(string botCommant, ITelegramBotClient botClient, Update update, string parameters);
    }
}
