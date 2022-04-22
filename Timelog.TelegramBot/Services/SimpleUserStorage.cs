using Timelog.TelegramBot.Interfaces;

namespace Timelog.TelegramBot.Services
{
    public class SimpleUserStorage : IUserStorage
    {
        private Dictionary<long, string> _storage = new Dictionary<long, string>();
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
        }
    }
}
