namespace Timelog.TelegramBot.Interfaces
{
    public interface IUserStorage
    {
        public string? GetTokenById(long id);
        public void SetTokenById(long id, string token);
    }
}
