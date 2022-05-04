using Newtonsoft.Json;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Models;

namespace Timelog.TelegramBot.Services
{
    /// <summary>
    ///     Реализация IChatStateStorage с хранением состояния в json файле
    /// </summary>
    public class SimpleChatStateStorage: IChatStateStorage
    {
        private Dictionary<long, ChatStateModel> _storage = new Dictionary<long, ChatStateModel>();

        public SimpleChatStateStorage()
        {
            try
            {
                string json = File.ReadAllText("scatStateStorage.json");
                _storage = JsonConvert.DeserializeObject<Dictionary<long, ChatStateModel>>(json) ?? new Dictionary<long, ChatStateModel>();
            }
            catch (Exception ex)
            {
                _storage = new Dictionary<long, ChatStateModel>();
            }
        }
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

        public void SetChatStateToChatId(long chatId, ChatStateModel chatState)
        {
            if (_storage.ContainsKey(chatId))
            {
                _storage[chatId] = chatState;
            }
            else
            {
                _storage.Add(chatId, chatState);
            }
            Save();
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(_storage, Formatting.Indented);
            File.WriteAllText("scatStateStorage.json", json);
        }
    }
}
