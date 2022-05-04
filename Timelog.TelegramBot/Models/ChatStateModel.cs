namespace Timelog.TelegramBot.Models
{
    public class ChatStateModel
    {
        public long ChatId { get; set; }
        public string CurrentCommand
        {
            get
            {
                return _command ?? Command.INVALID_COMMAND;
            }
            set
            {
                _command = value;
            }
        }
        public string? ProjectId { get; set; }

        private string? _command;
    }
}
