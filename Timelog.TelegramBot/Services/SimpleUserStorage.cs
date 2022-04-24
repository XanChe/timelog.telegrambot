using Newtonsoft.Json;
using Timelog.TelegramBot.Interfaces;

namespace Timelog.TelegramBot.Services
{
    public class SimpleUserStorage : IUserStorage
    {
        private Dictionary<long, string> _storage;
        
        public SimpleUserStorage()
        {
            try
            {
                string json = File.ReadAllText("userStorage.json");
                _storage = JsonConvert.DeserializeObject<Dictionary<long, string>>(json) ?? new Dictionary<long, string>();
            }
            catch (Exception ex)
            {
                _storage = new Dictionary<long, string>();
            }
            
        }
        public string? GetTokenById(long id)
        {
            if (_storage.ContainsKey(id))
            {
                return _storage[id];
            }
            else
            {
                return null;
            }
        }

        public void RemoveTokenById(long id)
        {
            if (_storage.ContainsKey(id))
            {
                _storage.Remove(id);
                Save();
            }
        }

        public void SetTokenById(long id, string token)
        {
            if (_storage.ContainsKey(id))
            {
                _storage[id] = token;
            }
            else
            {
                _storage.Add(id, token);
            }
            Save();
        }

        private void Save()
        {
            string json = JsonConvert.SerializeObject(_storage, Formatting.Indented);
            File.WriteAllText("userStorage.json", json);
        }

    }
}
