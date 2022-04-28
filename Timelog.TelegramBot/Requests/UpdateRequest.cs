using Telegram.Bot.Types;

namespace Timelog.TelegramBot.Requests
{
    public class UpdateRequest
    {
        public string MessageText { get; set; }
        public string Command { get; set; } = string.Empty;
        public string? ParametrString { get; set; }
        public List<string> Args { get; set; }
        public long TelegramChatId { get; }
        public long TelegramUserId { get; }
        public string? ErrorMessage { get; set; }
        public bool IsUserSingIn { get; set; } = false;
        private string? userAuthString;
        
        public UpdateRequest(Update update)
        {
            

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                Message message = update.Message;
                TelegramChatId = message.Chat.Id;
                TelegramUserId = message.From.Id;
                MessageText = message.Text ?? "";
            }
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                TelegramChatId = update.CallbackQuery.Message.Chat.Id;
                TelegramUserId = update.CallbackQuery.From.Id;
                MessageText = update.CallbackQuery.Data ?? "";
            }

            var commandRow = MessageText.Split(' ', 2);
            Command = commandRow[0].ToLower();
            Command = Command.Split('@')[0];
            ParametrString = commandRow.Length > 1 ? commandRow[1] : "";
            Args = commandRow.Length > 1 ? ParametrString.Split().ToList() : new List<string>();
        }
        public bool IsCommand()
        {
            return MessageText.Length > 1 && MessageText?[0] == '/';
        }
       
    }
}
