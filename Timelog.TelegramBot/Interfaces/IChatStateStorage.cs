using Timelog.TelegramBot.Models;

namespace Timelog.TelegramBot.Interfaces
{
    /// <summary>
    ///     Интерфейс "Сессии" для чата.
    /// </summary>
    public interface IChatStateStorage
    {
        public ChatStateModel? GetChatStateByChatId(long? chatId);
        public void SetChatStateToChatId(long chatId, ChatStateModel chatState);
    }
}
