using Telegram.Bot;
using Telegram.Bot.Types;

namespace Timelog.TelegramBot.Interfaces
{
    public delegate Task CommandHandler(ITelegramBotClient botClient, Update update, string parameters);
    public interface IBotCommandsService
    {
        public void RegisterHandler(string botCommant, CommandHandler command);
        public void UnregisterHandler(string botCommant);
        public Task ExecuteCommandAsync(string botCommant, ITelegramBotClient botClient, Update update, string parameters);
    }
}
