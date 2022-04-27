using Telegram.Bot.Types;

namespace Timelog.TelegramBot.Requests
{
    public class CommandRequest
    {
        public string MessageText { get; set; }
        public string Command { get; set; } = string.Empty;
        public string? ParametrString { get; set; }
        public long TelegramChatId { get; }
        public long TelegramUserId { get; }
        public string ErrorMessage { get; set; }
        public bool IsUserSingIn { get; set; } = false;
        private string? userAuthString;
        
        public CommandRequest(Message message)
        {
            TelegramChatId = message.Chat.Id;
            TelegramUserId = message.From.Id;
            MessageText = message.Text ?? "";
            var commandRow = MessageText.Split(' ', 2);
            Command = commandRow[0].ToLower();
            ParametrString = commandRow.Length > 1 ? commandRow[1] : "";
        }
        public bool IsCommand()
        {
            return MessageText.Length > 1 && MessageText?[0] == '/';
        }
       
    }
}
