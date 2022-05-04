using Timelog.TelegramBot.Models;

namespace Timelog.TelegramBot.Interfaces
{
    /// <summary>
    ///  Интерфейс сервиса регистрации и обработки комманд телеграм бота.
    /// </summary>
    public interface IBotCommandsService
    {
        public void RegisterCommand(string botCommant, Command command);
        public void UnregisterCommand(string botCommant);
        public Command? GetCommand(string botCommant);
    }
}
