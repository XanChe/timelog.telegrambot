using Telegram.Bot;
using Telegram.Bot.Types;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Models
{
    public delegate Task CommandHandler(ITelegramBotClient botClient, Update update, string parameters);
    public delegate Task<bool> ValidateHandler(CommandRequest commandRequest);
    public class Command
    {
        private  CommandHandler? _handler;
        private ValidateHandler? _validateHandler;
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

        public async Task<bool> Validation(CommandRequest commandRequest)
        {
            if (_validateHandler != null)
            {
                return await _validateHandler(commandRequest);
            }
            return true;
        }
        public async Task Execute(ITelegramBotClient botClient, Update update, string parametr)
        {
            if (_handler != null)
            {
                await _handler(botClient, update, parametr);
            }
        }
        public bool IsAuthorizationRequired()
        {
            return true;
        }
    }
}
