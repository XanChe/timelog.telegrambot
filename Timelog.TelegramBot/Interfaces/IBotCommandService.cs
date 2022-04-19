using Telegram.Bot.Types;

namespace Timelog.TelegramBot.Interfaces
{
    public interface IBotCommandService 
    {
        public void RegisterHandler(string botCommant, Action<Update> action);
        public void UnregisterHandler(string botCommant);
        public void UpdateHandler(string botCommant);
        public void ExecuteCommand(string botCommant, Update update);
    }
}
