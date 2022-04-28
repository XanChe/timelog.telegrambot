namespace Timelog.TelegramBot.Models
{
    public class ChatStateModel
    {
        public long ChatId { get; set; }
        public string? CurrentCommand { get; set; }
        public string? ProjectId { get; set; }
        
    }
}
