using Telegram.Bot;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Models
{
    public delegate Task CommandHandler(ITelegramBotClient botClient, UpdateRequest updateRequest);
    public delegate Task<bool> ValidateHandler(UpdateRequest updateRequest);
    public class Command
    {
        public const string INVALID_COMMAND = "/invalid_command";

        public string Name { get; }
        public Command(string name)
        {
            Name = name;
        }

        public void SetCommandHandler(CommandHandler? handler)
        {
            _handler = handler;
        }

        public void SetValidateHandler(ValidateHandler validateHandler)
        {
            _validateHandler = validateHandler;
        }

        public async Task<bool> Validation(UpdateRequest commandRequest)
        {
            if (_validateHandler != null)
            {
                return await _validateHandler(commandRequest);
            }
            return true;
        }
        public async Task Execute(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            if (_handler != null)
            {
                await _handler(botClient, updateRequest);
            }
        }
        public bool IsAuthorizationRequired()
        {
            return true;
        }

        private CommandHandler? _handler;
        private ValidateHandler? _validateHandler;
    }
}
