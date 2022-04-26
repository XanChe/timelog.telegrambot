namespace Timelog.TelegramBot.Interfaces
{
    public interface IUserStorage
    {
        public string? GetTokenByUserId(long id);
        public void SetTokenByUserId(long id, string token);
        public void RemoveTokenByUserId(long id);
    }
}
