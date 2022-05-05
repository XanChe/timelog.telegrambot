using Telegram.Bot;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Models
{
    /// <summary>
    /// Делегат обработчика выполнения команды телеграм бота
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="updateRequest">Запрос на изменение чата</param>
    /// <returns></returns>
    public delegate Task CommandExecuteHandler(ITelegramBotClient botClient, UpdateRequest updateRequest);
    /// <summary>
    /// Делегат обработчика валидации телеграм бота
    /// </summary>
    /// <param name="updateRequest">Запрос на изменение чата</param>
    /// <returns>bool</returns>
    public delegate Task<bool> CommandValidateHandler(UpdateRequest updateRequest);
    public class Command
    {
        public const string INVALID_COMMAND = "/invalid_command";

        public string Name { get; }
        public Command(string name)
        {
            Name = name;
        }

        public void SetCommandHandler(CommandExecuteHandler? handler)
        {
            _handler = handler;
        }

        public void SetValidateHandler(CommandValidateHandler validateHandler)
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

        private CommandExecuteHandler? _handler;
        private CommandValidateHandler? _validateHandler;
    }
}
