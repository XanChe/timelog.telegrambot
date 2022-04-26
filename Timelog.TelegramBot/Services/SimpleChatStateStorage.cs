using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Models;

namespace Timelog.TelegramBot.Services
{
    public class SimpleChatStateStorage: IChatStateStorage
    {
        private Dictionary<long, ChatStateModel> _storage = new Dictionary<long, ChatStateModel>();

        public ChatStateModel? GetChatStateByChatId(long? chatId)
        {
            if (chatId == null)
            {
                return null;
            }
            if (_storage.ContainsKey(chatId ?? 0))
            {
                return _storage[chatId ?? 0];
            }
            else
            {
                return null;
            }
        }

        public void SetChatStateByChatId(long chatId, ChatStateModel chatState)
        {
            if (_storage.ContainsKey(chatId))
            {
                _storage[chatId] = chatState;
            }
            else
            {
                _storage.Add(chatId, chatState);
            }
        }
    }
}
