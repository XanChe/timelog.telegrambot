namespace Timelog.TelegramBot.Interfaces
{
    /// <summary>
    ///     Интерфейс сервиса авторизации и индетификации пользователя
    /// </summary>
    public interface IUserStorage
    {
        public string? GetTokenByUserId(long id);
        public void SetTokenByUserId(long id, string token);
        public void RemoveTokenByUserId(long id);
    }
}
